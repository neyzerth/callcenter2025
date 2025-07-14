public class SessionLogListViewModel :  JsonResponse
{
    public List<SessionLog> SessionLogs { get; set; }
    
    public static SessionLogListViewModel GetResponse(List<SessionLog> sessionLogs)
    {
        SessionLogListViewModel r = new SessionLogListViewModel();
        r.Status = 0;
        r.SessionLogs= sessionLogs;
        return r;
    }
}