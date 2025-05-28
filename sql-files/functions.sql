create function getTimeBetween (@startTime time, @endTime time)
	returns time
as
begin
	return DATEADD(s, DATEDIFF(second, @startTime, @endTime), 0);
end;