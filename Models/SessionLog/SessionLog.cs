using System.Data;
using Microsoft.Data.SqlClient;

public class SessionLog
{
    #region statement
        private static string select = @"
            SELECT id, idSession, dateTimeStart, dateTimeEnd, timeElapsed, idStatus, statusDescription
            FROM viewSessionLog";
        
        private static string selectSession = select + " where idSession = @ID";
    #endregion
    
    #region attributes

    public int _id;
    public int _idSession;
    public SessionLogTime _time;
    public SessionLogStatus _status;

    #endregion
    
    #region propierties

    public int Id { get => _id; }

    public int IdSession { get => _idSession; set => _idSession = value; }

    public SessionLogTime Time { get => _time; set => _time = value ; }

    public SessionLogStatus Status { get => _status; set => _status = value; }

    #endregion
    
    #region constructors

    public SessionLog()
    {
        _id = 0;             
        _idSession = 0;   
        _time = null;         
        _status = null; 
    }

    public SessionLog(int id, int idSession, SessionLogTime time, SessionLogStatus status)
    {
        _id = id;
        _idSession = idSession;
        _time = time;
        _status = status;
    }

    #endregion
    
    #region class methods

    public static List<SessionLog> Get()
    {
        //command
        SqlCommand command = new SqlCommand(select + " order by dateTimeStart desc");
        
        //execute
        return SessionLogMapper.ToList(SqlServerConnection.ExecuteQuery(command));


    }
    
    public static List<SessionLog> GetSession(int idSession)
    {
        //Command
        SqlCommand command = new SqlCommand(selectSession);
        
        //parameters
        command.Parameters.AddWithValue("@ID", idSession);
        //execute
        return SessionLogMapper.ToList(SqlServerConnection.ExecuteQuery(command));
        
    }
    
    #endregion
    
}