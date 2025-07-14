using System.Data;
using Microsoft.Data.SqlClient;

public class Session
{
    #region statments

    private static string selectAll = "select * from viewSessions";
    private static string selectOne = "select * from viewSessions where id = @ID";
    
    #endregion
    
    #region atributes

    private int _id;
    private DateTime _signedIn;
    private TimeSpan _duration;
    private Agent _agent;
    private Station _station;

    #endregion
    
    #region properties
    
    public int Id { get => _id; }
    public DateTime SignedIn { get => _signedIn; }
    public TimeSpan Duration { get => _duration; }
    public Agent Agent { get => _agent; }
    public Station Station { get => _station; }
    
    #endregion
    
    #region constructors

    public Session(int id, DateTime signedIn, TimeSpan duration, Agent agent, Station station)
    {
        _id = id;
        _signedIn = signedIn;
        _duration = duration;
        _agent = agent;
        _station = station;
    }
    
    #endregion
    
    #region class methods


    public static Session Get(int id)
    {
        //command
        SqlCommand command = new SqlCommand(selectOne);
        
        //parameters
        command.Parameters.AddWithValue("@ID", id);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        if (table.Rows.Count > 0)
            return SessionMapper.ToObject(table.Rows[0]);
        
        throw new Exception();
    }

    public static int Login(int agentId, int pin, int stationId)
    {
        //command
        SqlCommand command = new SqlCommand("spLoginAgent");
        command.Parameters.AddWithValue("@agentId", agentId);
        command.Parameters.AddWithValue("@agentPin", pin);
        command.Parameters.AddWithValue("@stationId", stationId);
        
        //execute
        return SqlServerConnection.ExecuteProcedure(command);
        
    }
    public static int Logout(int agentId)
    {
        //command
        SqlCommand command = new SqlCommand("spLogoutAgent");
        command.Parameters.AddWithValue("@agentId", agentId);
        
        //execute
        return SqlServerConnection.ExecuteProcedure(command);
        
    }
    
    #endregion
}
