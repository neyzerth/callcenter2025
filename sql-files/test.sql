-- recieve call
declare @result as int;
exec spReceiveCall
	@phoneNumber = '6648337283',
	@status = @result output;
select @result;

--login agent
declare @result as int;
exec spLoginAgent
	@agentId = 1005,
	@agentPin = 2005,
	@stationId = 5,
	@status = @result output;
select @result;

--end call
declare @result as int;
exec spEndCall
	@callId = 2,
	@statusEndId = 4, -- 1: Customer Ended, 2: Agent Ended, 3: Call Dropped, 4: Tranfered (from table statusCallEnd)
	@status = @result output;
select @result;

-- calls
select * from viewCalls;
--sessions
select * from viewSessions;

describe table viewSessions;
-- session log
select * from viewSessionLog;


DROP VIEW viewCalls;

-- calls
create view viewCalls as
select c.id as call_id, c.phoneNumber, c.dateTimeReceived, c.dateTimeAnswered, c.dateTimeEnded,
dbo.getTimeBetween(c.dateTimeReceived, c.dateTimeAnswered) as timeInQueue, 
dbo.getTimeBetween(c.dateTimeAnswered, c.dateTimeEnded) as timeInCall,
c.idStatus AS call_idStatus, sc.description as call_status,
s.idAgent AS agent_id, a.name AS agent_name, s.idStation AS station_id 
from calls as c 
join statusCall as sc on c.idStatus = sc.id
left join sessions as s on c.idSession = s.id
left join agents as a on s.idAgent = a.id;
	
go

SELECT * FROM viewCalls;


select id as agent_id, name as agent_name, photo as agent_photo, pin as agent_pin 
           from agents where id=1001;