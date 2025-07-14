using System.Data;

public class CallMapper
{
    public static Call ToObject(DataRow row)
    {
        int id = (int)row["call_id"];
        string phoneNumber = (string)row["phoneNumber"];
        DateTime dateTimeReceived = (DateTime)row["dateTimeReceived"];
        //possible null values
        Object dateTimeAnswered =row["dateTimeAnswered"];
        Object dateTimeEnded = row["dateTimeEnded"];
        Object timeInQueue = row["timeInQueue"];
        Object timeInCall = row["timeInCall"];
        
        int statusId = (int)row["call_idStatus"];
        string status = (string)row["call_status"];
        //possible null values
        Object agentId = row["agent_id"];
        Object agentName = row["agent_name"];
        Object stationId = row["station_id"];
        
        Agent a = new Agent();
        CallStatus st = new CallStatus();
        CallTime ct = new CallTime();
        Station s = new Station();;
        
        st.Id = statusId;
        st.Description = status;
        
        ct.DateTimeReceived = dateTimeReceived;
        //Prevent System.DBNull exception for null data from database
        //Call is answered
        if (st.Id >= 2)
        {
            a = new Agent((int)agentId, (string)agentName);
            s = Station.Get((int)stationId);
            ct.DateTimeAnswered =(DateTime) dateTimeAnswered;
            ct.TimeInQueue =(TimeSpan) timeInQueue;
        }
        //Call is ended
        if (st.Id == 3)
        {
            ct.DateTimeEnded =(DateTime) dateTimeEnded;
            ct.TimeInCall = (TimeSpan)timeInCall;
        }


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