using System.Data;
using Microsoft.Data.SqlClient;

public class Agent
{
    #region statement

    private static string select = @"
           select id as agent_id, name as agent_name, photo as agent_photo, pin as agent_pin 
           from agents ";

    private static string insert = @"INSERT INTO agents (id, name,photo, pin)
                            VALUES (@id, @name, @photo, @pin)";
    
    
    #endregion
    
    #region attributes

    private int _id;
    private string _name;
    private string _photo;
    private int _pin;

    #endregion

    #region properties

    public int Id { get => _id; }
    public string Name { get => _name; set => _name = value; }
    public string Photo { get => Config.Configuration.Paths.Domain + Config.Configuration.Paths.Photos.Agents + _photo; set => _photo = value; }
    public int Pin { set => _pin = value; }
    
    #endregion

    #region constructors

    /// <summary>
    /// Creates an empty Agent object
    /// </summary>
    public Agent()
    {
        _id = 0;
        _name = "";
        _photo = "";
        _pin = 0;
    }
    public Agent(int id, string name)
    {
        _id = id;
        _name = name;
        _photo = "";
        _pin = 0;
    }

    /// <summary>
    /// Creates an object with data from the arguments
    /// </summary>
    /// <param name="id">Agent id</param>
    /// <param name="name">Full name</param>
    /// <param name="photo">Photo URL</param>
    /// <param name="pin">Agent pin</param>
    public Agent(int id, string name, string photo, int pin)
    {
        _id = id;
        _name = name;
        _photo = photo;
        _pin = pin;
    }

    #endregion

    #region class methods
    
    /// <summary>
    /// Returns a list of all agents
    /// </summary>
    /// <returns></returns>
    public static List<Agent> Get() 
    {
        //command
        SqlCommand command = new SqlCommand(select + " order by name");
        
        //execute
        return AgentMapper.ToList(SqlServerConnection.ExecuteQuery(command));
        
    }

    /// <summary>
    /// Returns the agent with the specified id
    /// </summary>
    /// <param name="id">Agent id</param>
    /// <returns></returns>
    public static Agent Get(int id)
    {
        //command
        SqlCommand command = new SqlCommand(select + " where id = @ID");
        
        //parameters
        command.Parameters.AddWithValue("@ID", id);
        //execute
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        if (table.Rows.Count > 0)
            return AgentMapper.ToObject(table.Rows[0]);
        
        throw new AgentNotFoundException(id);

    }
    
    public static bool Insert(PostAgent p)
    {
        SqlCommand command  =  new SqlCommand(insert);
        
        //get the extension of file (.jpg, .png, .jpeg)
        string extension = p.Photo.FileName.Split('.').Last();
        //set the file name the agent id
        string photoFilename = p.Id + "." + extension;
        
        //parameters
        command.Parameters.AddWithValue("@id", p.Id);
        command.Parameters.AddWithValue("@name", p.Name);
        command.Parameters.AddWithValue("@photo", photoFilename);
        command.Parameters.AddWithValue("@pin", p.Pin);
        //execute, return if dont execute
        if (!SqlServerConnection.ExecuteCommand(command))
            return false; //error

        //get the path of photos in the system server
        string path = Config.Configuration.Paths.Server + Config.Configuration.Paths.Photos.Agents + photoFilename;
        try
        {
            //write the image in the disl
            using (FileStream stream = new FileStream(path, FileMode.Create))
            { p.Photo.CopyTo(stream); }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //error
            return false;
        }
        return true;
    }

    #endregion
}
