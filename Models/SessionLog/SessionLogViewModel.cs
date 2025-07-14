public class SessionLogViewModel
{
    public int Status { get; set; }
    public SessionLog SessionLog { get; set; }


    public static SessionLogViewModel GetResponse(SessionLog sessionLog)
    {
        SessionLogViewModel r = new SessionLogViewModel();
        r.Status = 0;
        r.SessionLog = sessionLog;
        return r;
    }
}