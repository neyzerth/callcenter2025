using System.Data;

public class SessionMapper
{
    public static Session ToObject(DataRow row)
    {
        //cast columns
        int id = (int)row["id"];
        DateTime signedIn = (DateTime)row["dateTimeLogin"];
        TimeSpan duration = (TimeSpan)row["timeLoggedIn"];
        int stationId = (int)row["idStation"];
        int agentId = (int)row["idAgent"];

        Agent a = Agent.Get(agentId);
        Station s = Station.Get(stationId);

        return new Session(id, signedIn, duration, a, s);
    }

    public static List<Session> ToList(DataTable table)
    {
        //create a list
        List<Session> list = new List<Session>();
        
        //populate list
        foreach (DataRow row in table.Rows)
        {
            list.Add(ToObject(row));
        }

        return list;
    }
}