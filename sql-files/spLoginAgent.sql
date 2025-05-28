create procedure spLoginAgent
	-- parameters
	@agentId int,
	@agentPin int,
	@stationId int,
	@status int output
as
begin
	-- variables
	declare @stationActive bit;
	declare @sessionLogStatus int;
	declare @sessionId int;
	declare @statusAssigned int;

	-- configuration
	select @sessionLogStatus = idDefaultSessionLogStatus from config;

	-- validate
	set @status = 0; -- no error
	if not exists(select id from agents where id = @agentId and pin = @agentPin) set @status = 1; -- could not log in agent (invalid id and/or pin)
	if exists(select id from sessions where idAgent = @agentId and active = 1) set @status = 2; --agent already logged in
	if not exists(select id from stations where id = @stationId) set @status = 3; -- invalid station id;
	select @stationActive = active from stations where id = @stationId;
	if @stationActive = 0 set @status = 4; -- station not active
	if exists(select id from sessions where idStation = @stationId and active = 1) set @status = 5; -- station in use
		
	if @status = 0 begin	
		--transaction
		begin transaction
			--try
			begin try
				-- start session
				insert into sessions (idAgent, idStation) values (@agentId, @stationId);
				-- get session id
				set @sessionId = SCOPE_IDENTITY();
				-- session log
				insert into sessionLog (idSession, idStatus) values (@sessionId, @sessionLogStatus);
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
	end;
	
end --end procedure


