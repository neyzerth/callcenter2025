using Microsoft.Data.SqlClient;

public class Call
{
    #region statement
    
    private static string selectAll="select * from viewCalls";
    private static string selectOne="select * from viewCalls where id = @ID";
    
    #endregion
    
    #region attributes
    
    private int _id;
    private string _phoneNumber;
    private CallTime _callTime;
    private Status _status;
    private Agent _agent;
    private Station _station;

    #endregion
    
    #region propierties

    public int Id { get => _id; }

    public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value ; }

    public CallTime CallTime { get => _callTime; set => _callTime = value;}

    public Status Status { get => _status; set => _status = value; }

    public Agent Agent { get => _agent; set => _agent = value ; }

    public Station Station { get => _station; set => _station = value; }

    #endregion
    
    #region constructors

    public Call(int id, string phoneNumber, CallTime callTime, Status status, Agent agent, Station station)
    {
        _id = id;
        _phoneNumber = phoneNumber;
        _callTime = callTime;
        _status = status;
        _agent = agent;
        _station = station;
    }

    #endregion
    
    #region class methods

    public static List<Call> Get()
    {
        SqlCommand command = new SqlCommand(selectAll);
        
        return CallMapper.ToList(SqlServerConnection.ExecuteQuery(command));
    }
    
    public static Call Get(int id)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@ID", id);
        
        return CallMapper.ToObject(SqlServerConnection.ExecuteQuery(command).Rows[0]);
    }
    
    #endregion
}