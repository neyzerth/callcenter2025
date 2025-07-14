using System.Data;
using Microsoft.Data.SqlClient;

public class Call
{
    #region statments

    private static string selectAll = "SELECT * FROM viewCalls";

    #endregion

    #region attributes

    private int _id;
    private string _phoneNumber;
    private CallTime _time;
    private CallStatus _status;
    private Agent _agent;
    private Station _station;

    #endregion

    #region properties

    public int Id { get => _id; }
    public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
    public CallTime Time { get => _time; }
    public CallStatus Status { get => _status; set => _status = value; }
    public Agent Agent { get => _agent; }
    public Station Station { get => _station; }

    #endregion

    #region constructors
    public Call()
    {
        _id = 0;
        _phoneNumber = "";
        _time = new CallTime();
        _status = new CallStatus();
        _agent = new Agent();
        _station = new Station();
    }
    public Call(int id, string phoneNumber, CallTime time, CallStatus status, Agent agent, Station station)
    {
        _id = id;
        _phoneNumber = phoneNumber;
        _time = time;
        _status = status;
        _agent = agent;
        _station = station;
    }

    #endregion

    #region class methods

    /// <summary>
    /// Returns a list of all calls
    /// </summary>
    /// <returns></returns>
    public static List<Call> Get()
    {
        //Command
        SqlCommand command = new SqlCommand(selectAll);
        //La query retorna una tabla, la procesa para retornar una lista
        return CallMapper.ToList(SqlServerConnection.ExecuteQuery(command));
    }

    public static int ReceiveCall(string phoneNumber)
    {
        //command
        SqlCommand command = new SqlCommand("spReceiveCall");
        //parameters
        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

        //execute
        return SqlServerConnection.ExecuteProcedure(command);
    }

    public static int EndCall(int callId, int statusEndId)
    {
        //command
        SqlCommand command = new SqlCommand("spEndCall");
        //parameters
        command.Parameters.AddWithValue("@callId", callId);
        command.Parameters.AddWithValue("@statusEndId", statusEndId);

        //execute
        return SqlServerConnection.ExecuteProcedure(command);
    }

    public static int EndCallRandom()
    {
        //command
        SqlCommand command = new SqlCommand("spEndCallRandom");

        //execute
        return SqlServerConnection.ExecuteProcedure(command);
    }

    public static CallDuration CallDurationByDate(DateTime date)
    {
        //query
        string query = @"
            select duration, count(*) as total
            from viewCallTotals 
            where date = @date and idStatus = 3 
            group by duration 
            order by duration;";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        //return object
        CallDuration result = new CallDuration();
        result.Minutes = new int[table.Rows.Count];
        result.Totals = new int[table.Rows.Count];
        //index
        int index = 0;
        //read data
        foreach (DataRow row in table.Rows)
        {
            result.Minutes[index] = (Int32)row["duration"];
            result.Totals[index] = (Int32)row["total"];
            index++;
        }
        //return result
        return result;
    }

    public static CallTotal CallTotalsByHour(DateTime date)
    {
        //query
        string query = @"
            select hour, count(*) as total 
            from viewCallTotals 
            where date = @date 
            group by hour 
            order by hour";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        //return object
        CallTotal result = new CallTotal();
        result.Hour = new int[24];
        result.Totals = new int[24];
        //24 hours
        for (int h = 0; h < 24; h++)
        {
            result.Hour[h] = h;
            result.Totals[h] = 0;
        }
        //read data
        foreach (DataRow row in table.Rows)
            result.Totals[(Int32)row["hour"]] = (Int32)row["total"];
        //return result
        return result;
    }

    public static Metric CallsInQueue(DateTime date)
    {
        //response
        Metric m = new Metric();
        //query
        string query = "SELECT COUNT(*) AS callsInQueue FROM calls WHERE CONVERT(date, dateTimeReceived) = @date AND idStatus = 1";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        //read data
        if (table.Rows.Count > 0)
        {
            m.Value = table.Rows[0]["callsInQueue"].ToString();
            int value = int.Parse(m.Value);
            if (value > 0) m.Status = MetricStatus.GOOD.ToString();
            if (value > 5) m.Status = MetricStatus.LOW.ToString();
            if (value > 10) m.Status = MetricStatus.MID.ToString();
            if (value > 15) m.Status = MetricStatus.HIGH.ToString();
            if (value > 25) m.Status = MetricStatus.EXTREME.ToString();

        }
        //return m
        return m;
    }
    public static int ActiveCalls(DateTime date)
    {
        //query
        string query = "SELECT COUNT(*) AS activeCalls FROM calls WHERE CONVERT(date, dateTimeReceived) = @date AND idStatus = 2";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        int result = 0;
        //read data
        if (table.Rows.Count > 0)
            result = (Int32)table.Rows[0]["activeCalls"];
        //return result
        return result;
    }
    public static int TotalCalls(DateTime date)
    {
        //query
        string query = "SELECT COUNT(*) AS totalCalls FROM calls WHERE CONVERT(date, dateTimeReceived) = @date";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        int result = 0;
        //read data
        if (table.Rows.Count > 0)
            result = (Int32)table.Rows[0]["totalCalls"];
        //return result
        return result;
    }
    public static int ActiveSessions()
    {
        //query
        string query = "SELECT COUNT(*) AS activeSessions FROM viewSessions";
        //command
        SqlCommand command = new SqlCommand(query);
        
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        int result = 0;
        //read data
        if (table.Rows.Count > 0)
            result = (Int32)table.Rows[0]["activeSessions"];
        //return result
        return result;
    }
    public static Metric AverageHandleTime(DateTime date)
    {
        //query
        string query = "SELECT AVG(duration) AS average FROM viewCalls WHERE CONVERT(date, dateTimeEnded) = @date";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        int result = 0;
        //read data
        
        Metric m = new Metric();
        if (table.Rows.Count > 0)
        {
            int value = (int)table.Rows[0]["average"];
            m.Value = TimeSpan.FromMinutes(value).ToString();
            if (value > 0) m.Status = MetricStatus.GOOD.ToString();
            if (value > 5) m.Status = MetricStatus.LOW.ToString();
            if (value > 15) m.Status = MetricStatus.MID.ToString();
            if (value > 20) m.Status = MetricStatus.HIGH.ToString();
            if (value > 30) m.Status = MetricStatus.EXTREME.ToString();
        }

        //return result
        return m;
    }
    public static Metric WaitTime(DateTime date)
    {
        //query
        string query = "SELECT MAX(timeInQueue) AS waitTime FROM viewCalls WHERE CONVERT(date, dateTimeAnswered) = " +
                       "@date AND call_status = 'Answered'";
        //command
        SqlCommand command = new SqlCommand(query);
        //parameters
        command.Parameters.AddWithValue("@date", date);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        Metric m = new Metric();
        //read data
        if (table.Rows.Count > 0)
        {
            if (table.Rows[0]["waitTime"] == DBNull.Value)
                return new Metric();
            
            TimeSpan ts = (TimeSpan)table.Rows[0]["waitTime"];
            m.Value = ts.ToString();
            int value = (int)ts.TotalMinutes;
            if (value > 0) m.Status = MetricStatus.GOOD.ToString();
            if (value > 3) m.Status = MetricStatus.LOW.ToString();
            if (value > 8) m.Status = MetricStatus.MID.ToString();
            if (value > 15) m.Status = MetricStatus.HIGH.ToString();
            if (value > 25) m.Status = MetricStatus.EXTREME.ToString();
        }
        //return result
        return m;
    }
    #endregion
}