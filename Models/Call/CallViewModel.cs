public class CallViewModel
{
    public int Status { get; set; }
    public Call Call { get; set; }


    public static CallViewModel GetResponse(Call call)
    {
        CallViewModel r = new CallViewModel();
        r.Status = 0;
        r.Call = call;
        return r;
    }
}
