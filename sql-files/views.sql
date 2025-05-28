-- calls
create view viewCalls as
	select c.id as callId, c.phoneNumber, c.dateTimeReceived, c.dateTimeAnswered, c.dateTimeEnded,
	dbo.getTimeBetween(c.dateTimeReceived, c.dateTimeAnswered) as timeInQueue, 
	dbo.getTimeBetween(c.dateTimeAnswered, c.dateTimeEnded) as timeInCall,
	c.idStatus, sc.description as callStatus,
	s.idAgent, a.name, s.idStation 
	from calls as c 
	join statusCall as sc on c.idStatus = sc.id
	left join sessions as s on c.idSession = s.id
	left join agents as a on s.idAgent = a.id;
	
go

-- sessions
create view viewSessions as
	select s.id, s.dateTimeLogin, 
	dbo.getTimeBetween(s.dateTimeLogin, GETDATE()) as timeLoggedIn,
	s.idAgent, a.name as agentName, s.idStation, s.idCurrentCall, c.phoneNumber, c.dateTimeAnswered,
	dbo.getTimeBetween(c.dateTimeAnswered, GETDATE()) as timeInCall
	from sessions as s 
	join agents as a on s.idAgent = a.id
	left join calls as c on s.idCurrentCall = c.id

go
	
-- session log
create view viewSessionLog as
	select sl.id, sl.idSession, sl.dateTimeStart, sl.dateTimeEnd,
	dbo.getTimeBetween(sl.dateTimeStart, sl.dateTimeEnd) as timeElapsed,
	sl.idStatus, ssl.description as statusDescription
	from sessionLog as sl
	join statusSessionLog as ssl on sl.idStatus = ssl.id

go
	
-- available agent/session
create view viewAvailableSession as
	select s.id as sessionId, sl.id as sessionLogId, sl.dateTimeStart 
	from sessions as s 
	join sessionLog as sl on s.id = sl.idSession 
	join statussessionlog as stsl on sl.idStatus = stsl.id 
	where s.active = 1 and stsl.available = 1 and dateTimeEnd is null;