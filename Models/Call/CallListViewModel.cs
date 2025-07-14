public class CallListViewModel : JsonResponse
{
    public List<Call> Calls { get; set; }
    
    public static CallListViewModel GetResponse(List<Call> calls)
    {
        CallListViewModel r = new CallListViewModel();
        r.Status = 0;
        r.Calls = calls;
        return r;
    }
}
