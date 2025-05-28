using System.Data;

public class CallMapper
{
    public static Call ToObject(DataRow row)
    {
        int id = (int)row["id"];
        string phoneNumber = (string)row["phoneNumber"];
        DateTime dateTimeReceived = (DateTime)row["dateTimeReceived"];
        DateTime dateTimeAnswered = (DateTime)row["dateTimeAnswered"];
        DateTime dateTimeEnded = (DateTime)row["dateTimeEnded"];
        TimeSpan timeInQueue = (TimeSpan)row["timeInQueue"];
        TimeSpan timeInCall = (TimeSpan)row["timeInCall"];
        int status = (int)row["status"];
        int agentId = (int)row["idAgent"];
        int stationId = (int)row["idStation"];
        
        Agent a = Agent.Get(agentId);
        Station s = Station.Get(stationId);
        Status st = Status.Get(status);
        CallTime ct = new CallTime();
        ct.DateTimeAnswered = dateTimeAnswered;
        ct.DateTimeEnded = dateTimeEnded;
        ct.DateTimeReceived = dateTimeReceived;
        ct.TimeInCall = timeInCall;
        ct.TimeInQueue = timeInQueue;

        return new Call(id, phoneNumber, ct, st,a, s);
    }
    
    public static List<Call> ToList(DataTable table)
    {
        List<Call> list = new List<Call>();
        
        foreach (DataRow row in table.Rows)
        {
            list.Add(ToObject(row));
        }
        
        return list;
    }
}