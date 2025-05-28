create procedure spAssignCall
	-- parameters
	@status int output
as
begin
	-- variables
	declare @callReceiveStatus int;
	declare @callAnswerStatus int;
	declare @callId int;
	declare @sessionId int;
	declare @sessionLogId int;
	declare @dateTimeStart dateTime;
	declare @callWaitTime time;
	declare @dateTimeAnswered as dateTime;
	declare @dateTimeReceived as dateTime;

	-- validation
	set @status = 0;
	
	-- configuration
	select @callReceiveStatus = idDefaultCallReceiveStatus, @callAnswerStatus = idDefaultCallAnswerStatus from config;

	-- start transaction
	begin transaction;

	--try
	begin try
		-- find calls in queue with longest wait
		select top(1) @callId = id from calls where idStatus = @callReceiveStatus order by datetimeReceived asc; 
		if @@ROWCOUNT > 0 begin
			-- find logged in agent with longest idle time 
			select top(1) @sessionId = sessionId, @sessionLogId = sessionLogId, @dateTimeStart = dateTimeStart from viewAvailableSession order by dateTimeStart asc;
			if @@ROWCOUNT > 0 begin
				-- assign call to session
				update sessions set idCurrentCall = @callId where id = @sessionId;
				-- update log (finish previous activity)
				update sessionLog set dateTimeEnd = GETDATE() where id = @sessionLogId;
				-- new entry in session log
				insert into sessionLog (idSession, idStatus) values (@sessionId, @callAnswerStatus);
				-- update call
				update calls set dateTimeAnswered = GETDATE(), idSession = @sessionId, idStatus = @callAnswerStatus where id = @callId;
				-- wait time
				select @dateTimeAnswered = dateTimeAnswered, @datetimeReceived = datetimeReceived from calls where id = @callId;
				set @callWaitTime = DBO.getTimeBetween(@dateTimeAnswered, @datetimeReceived);
				--daily totals
				
			end;
		end;
		
		--commit transaction
		commit transaction;
	end try
	begin catch
		-- rollback changes
		rollback transaction;
		--result
		set @status = 999;
	end catch
	
	-- return
	select @status;
	
end --end procedure