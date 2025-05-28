create procedure spReceiveCall
	-- parameters
	@phoneNumber varchar(10),
	@status int output
as
begin
	-- variables
	declare @callStatus int;
	declare @statusAssigned int;

	-- configuration
	select @callStatus = idDefaultCallReceiveStatus from config;
		
	--transaction
	begin transaction
		--try
		begin try
			-- receive call
			insert into calls (phoneNumber, idStatus) values (@phoneNumber, @callStatus);
	
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
