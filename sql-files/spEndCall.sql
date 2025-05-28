create procedure spEndCall
	-- parameters
	@callId int,
	@statusEndId int,
	@status int output
as
begin
	-- variables
	declare @callAnswerStatus int;
	declare @callEndStatus int;
	declare @callStatus int;
	declare @sessionId int;
	declare @sessionLogId int;
	declare @sessionLogStatus int;
	declare @callHandleTime time;
	declare @statusAssigned int;

	-- configuration
	select @callAnswerStatus = idDefaultCallAnswerStatus, @callEndStatus = idDefaultCallEndStatus, @sessionLogStatus = idDefaultSessionLogStatus from config;
	
	-- validate
	set @status = 0; -- no error
	select @sessionId = idSession, @callStatus = idStatus from calls where id = @callId;
	if @@ROWCOUNT = 0
	   set @status = 1; -- invalid call id
	else begin
		if @callStatus <> 2 set @status = 2; -- call is not active 
	end;
	if not exists(select id from statusCallEnd where id = @statusEndId) set @status = 3; -- call end status does not exist
	
	--transaction
	begin transaction
		--try
		begin try
			-- update call
			update calls set dateTimeEnded = GETDATE(), idStatus = @callEndStatus, idStatusEnd = @statusEndId where id = @callId;
			-- update session
			update sessions set idCurrentCall = null where id = @sessionId;
			-- update log (finish previous activity)
			select top (1) @sessionLogId = id from sessionLog where idSession = @sessionId and dateTimeEnd is null order by id desc;
			update sessionLog set dateTimeEnd = GETDATE() where id = @sessionLogId;
			-- new entry in session log
			insert into sessionLog (idSession, idStatus) values (@sessionId, @sessionLogStatus);
			-- add to totals
			-- handle time
			--select sec_to_time(time_to_sec(timediff(dateTimeEnded, dateTimeAnswered))) into callHandleTime from calls where id = callId;
			-- totals
			--update callHourlyTotals set callsEnded = callsEnded + 1, handleTime = handleTime + callHandleTime where date = date(now()) and hour = hour(now());
			-- if new day
		
			-- assign call 
			exec spAssignCall @statusAssigned output;
			if @statusAssigned = 0 
				--commit transaction
				commit transaction;
			else
				-- rollback changes
				rollback transaction;
		end try
		begin catch
			-- rollback changes
			rollback transaction;
			--result
			set @status = 999;
		end catch;
	
	-- return
	select @status;
	
end --end procedure