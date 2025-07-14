using System.Data;
using CallCenter.Models.Stations;
using Microsoft.Data.SqlClient;

public class Station
{
    #region statement
    private static string select = @"
       select id, rowNumber, deskNumber, ipAddress, active
       from stations 
                                  ";

    private static string insert = @"
        insert into stations (id, rowNumber, deskNumber, ipAddress, active)
        values (@id, @row, @desk, @ip, @active);
";

    #endregion
    
    #region attributes
    
    private int _id;
    private StationLocation _location;
    private string _ipAddress;
    private bool _active;

    #endregion
    
    #region properties
    
    public int Id { get => _id; }
    public StationLocation Location { get => _location; set => _location = value; }
    public string IpAddress { get => _ipAddress; set => _ipAddress = value; }
    public bool Active { get => _active; set => _active = value; }
    
    #endregion
    
    #region constructors

    public Station(int id, int rowNumber, int deskNumber, string ipAddress, bool active)
    {
        _id = id;
        _location = new StationLocation();
        _location.Row = rowNumber;
        _location.Desk = deskNumber;
        _ipAddress = ipAddress;
        _active = active;
    }
    
    public Station()
    {
        _id = 0;
        _location = new StationLocation();
        _ipAddress = "0.0.0.0";
        _active = true;
    }

    #endregion
    
    #region class methods

    /// <summary>
    /// Returns a list of all Stations
    /// </summary>
    /// <returns></returns>
    public static List<Station> Get()
    {
        //Command
        SqlCommand command = new SqlCommand(select + " order by rowNumber, deskNumber");
        
        //execute
        return StationMapper.ToList(SqlServerConnection.ExecuteQuery(command));
        
    }

    /// <summary>
    /// Returns the station with the specified id
    /// </summary>
    /// <param name="id">Station id</param>
    /// <returns></returns>
    public static Station Get(int id)
    {
        //Command
        SqlCommand command = new SqlCommand(select + " where id = @ID");
        
        //parameters
        command.Parameters.AddWithValue("@ID", id);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        if (table.Rows.Count > 0)
            return StationMapper.ToObject(table.Rows[0]);
        
        throw new StationNotFoundException(id);
    }

    public static bool Insert(PostStation p)
    {
        //Station s = new Station(p.Id, p.Desk, p.Row, p.Ip, p.Active);
        SqlCommand command  =  new SqlCommand(insert);
        
        //parameters
        command.Parameters.AddWithValue("@id", p.Id);
        command.Parameters.AddWithValue("@desk", p.Desk);
        command.Parameters.AddWithValue("@row", p.Row);
        command.Parameters.AddWithValue("@ip", p.Ip);
        command.Parameters.AddWithValue("@active", p.Active);
        //execute
        return  SqlServerConnection.ExecuteCommand(command);
    }

    #endregion
}