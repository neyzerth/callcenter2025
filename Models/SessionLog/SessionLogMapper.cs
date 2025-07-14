using System.Data;

public class SessionLogMapper
{
    public static SessionLog ToObject(DataRow row)
    {
        //cast columns
        int id = (int)row["id"];
        int sessionId = (int)row["idSession"];
        DateTime dateTimeStart = (DateTime)row["dateTimeStart"];
        int statusId = (int)row["idStatus"];
        string statusDescription = (string)row["statusDescription"];
        
        // columnas que pueden ser nulas
        DateTime? dateTimeEnd = row["dateTimeEnd"] != DBNull.Value 
            ? (DateTime?)row["dateTimeEnd"] : null;

        TimeSpan? timeElapsed = row["timeElapsed"] != DBNull.Value 
            ? (TimeSpan?)row["timeElapsed"] : null;
        
        SessionLogTime t = new SessionLogTime();
        SessionLogStatus st = new SessionLogStatus();
        
        t.DateTimeStart = dateTimeStart;
        t.DateTimeEnd = dateTimeEnd;
        t.TimeElapsed = timeElapsed;
        st.Id = statusId;
        st.Description = statusDescription;
        
        return new SessionLog(id, sessionId, t, st);
    }

    public static List<SessionLog> ToList(DataTable table)
    {
        //create a list
        List<SessionLog> list = new List<SessionLog>();
        
        //populate list
        foreach (DataRow row in table.Rows)
        { 
            list.Add(ToObject(row));
        }

        return list;
    }
}