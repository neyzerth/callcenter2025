using System.Data;
using Microsoft.Data.SqlClient;

public class Status
{
    #region statements

    private static string selectOne = "SELECT * FROM statusCall WHERE id = @ID";
    #endregion
    
    #region propierties

    private int _id;
    private string _description;
    private bool _availableToAnswer;
    
    #endregion
    
    #region propierties

    public int Id { get => _id; }

    public string Description { get => _description; set => _description = value ?? throw new ArgumentNullException(nameof(value)); }

    public bool AvailableToAnswer { get => _availableToAnswer; set => _availableToAnswer = value; }

    #endregion
    
    #region constructors

    public Status(int id, string description, bool availableToAnswer)
    {
        _id = id;
        _description = description;
        _availableToAnswer = availableToAnswer;
    }

    #endregion
    
    #region class methods

    public static Status Get(int id)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@ID", id);
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        if (table.Rows.Count > 0)
            return StatusMapper.ToObject(table.Rows[0]);
        
        throw new Exception();
        
    }

    #endregion
}