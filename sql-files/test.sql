-- recieve call
declare @result as int;
exec spReceiveCall
	@phoneNumber = '6451234986',
	@status = @result output;
select @result;

select * from viewSessions;

--login agent
declare @result as int;
exec spLoginAgent
	@agentId = 1001,
	@agentPin = 2001,
	@stationId = 1,
	@status = @result output;
select @result;
select * from statusSessionLog;
select * from statusCall;
select * from statusCallEnd;



--end call
declare @result as int;
exec spEndCall
	@callId = 1005,
	@statusEndId = 1, -- 1: Customer Ended, 2: Agent Ended, 3: Call Dropped, 4: Tranfered (from table statusCallEnd)
	@status = @result output;
select @result;

declare @result as int;
exec spLogoutAgent
	@agentId = 1005,
	@status = @result output;
select @result;

-- calls
select * from viewCalls;
--sessions
select * from viewSessions;

describe table viewSessions;
-- session log
select * from viewSessionLog;
select * from sessions;


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

select s.id, s.dateTimeLogin, 
	dbo.getTimeBetween(s.dateTimeLogin, GETDATE()) as timeLoggedIn,
	s.idAgent, a.name as agentName, s.idStation, s.idCurrentCall, c.phoneNumber, c.dateTimeAnswered,
	dbo.getTimeBetween(c.dateTimeAnswered, GETDATE()) as timeInCall
	from sessions as s 
	join agents as a on s.idAgent = a.id
	left join calls as c on s.idCurrentCall = c.id
	where s.dateTimeLogout is null;

-- Torres check

select * from agents;

select * from stations;

select * from viewCalls;

select * from viewSessionLog where idSession = 1;

select * from calls;

declare @result as int;
exec spEndCallRandom
	@status = @result output;


select * from viewCalls;

select call_id, timeInQueue from viewCalls where call_idStatus = 1 order by timeInQueue desc;

select call_status AS status, COUNT(*) as Now from viewCalls group BY call_status;

select duration minutes, count(duration) num from viewCalls group by duration;

select * from agents;

select * from viewAvailableSession vas ;

select count(*) from stations where active = 1;
select * from stations;

create view viewCallTotals as
select 
	convert(date, datetimeEnded) as date,
	datepart(hh, datetimeEnded) as hour,
	datediff(n, datetimeAnswered, datetimeEnded) as duration,
	idStatus
from calls;

declare @date date = '2025-06-18';
-- calls by hour
select hour, count(*) from viewCallTotals where date = @date GROUP by hour order by hour;
 


