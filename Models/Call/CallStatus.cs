using System.Data;
using Microsoft.Data.SqlClient;

public class CallStatus
{
    #region statements

    // private static string selectOne = "SELECT * FROM statusCall WHERE id = @ID";
    #endregion
    
    #region propierties

    public int Id { get; set;  }

    public string Description { get ; set; }

    public bool AvailableToAnswer { get; set; }

    #endregion
    
}