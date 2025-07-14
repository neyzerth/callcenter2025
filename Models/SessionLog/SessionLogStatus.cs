public class SessionLogStatus
{
    #region statement
    // private static string select = @"
    //         SELECT id, description, available
    //         FROM statusSessionLog";
    #endregion
    
    #region propierties
    
    public int Id { get; set; }
    public string Description { get; set; }
    public bool Available { get; set; }
    
    #endregion
    
}