public class ViewTotalsResponse : JsonResponse
{
    public string Date { get; set; } 
    public string LastUpdate { get; set; } 
    public CallDuration Duration { get; set; } 
    public CallTotal Totals { get; set; } 
    public int ActiveSessions { get; set; } 
    public Metric CallsInQueue { get; set; } 
    public int ActiveCalls { get; set; } 
    public int TotalCalls { get; set; } 
    public Metric WaitTime { get; set; } 
    public Metric AverageHandleTime { get; set; } 

    public static ViewTotalsResponse GetResponse(DateTime date)
    {
        ViewTotalsResponse r = new ViewTotalsResponse();
        r.Date = date.ToLongDateString();
        r.LastUpdate = DateTime.Now.ToLongTimeString();
        r.Duration = Call.CallDurationByDate(date);
        r.Totals = Call.CallTotalsByHour(date);
        r.CallsInQueue = Call.CallsInQueue(date);
        r.ActiveCalls = Call.ActiveCalls(date);
        r.TotalCalls = Call.TotalCalls(date);
        r.ActiveSessions = Call.ActiveSessions();
        r.AverageHandleTime = Call.AverageHandleTime(date);
        r.WaitTime = Call.WaitTime(date);
        return r;
    }


}