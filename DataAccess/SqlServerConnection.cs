using Microsoft.Data.SqlClient;
using System.Data;

public class SqlServerConnection
{
    #region variables
    
    private static string connectionString =
        "Data Source = " + Config.Configuration.SqlServer.Server + "; " +
        "Initial Catalog = " + Config.Configuration.SqlServer.Database + "; " +
        "User Id = " + Config.Configuration.SqlServer.User + "; " +
        "Password = " + Config.Configuration.SqlServer.Password + ";"+
        "TrustServerCertificate = " + Config.Configuration.SqlServer.TrustServerCertificate+";";  

    private static SqlConnection connection;
    
    #endregion

    #region class methods

    private static bool Open()
    {
        //result
        bool connected = false;
        //open
        try
        {
            //connection
            connection = new SqlConnection(connectionString);
            //open
            connection.Open();
            connected = true;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("ARGUMENT EXCEPTION: "+e);
        }
        catch (SqlException e)
        {
            Console.WriteLine("SQL EXCEPTION: "+e);
        }
        catch(Exception e)
        {
            Console.WriteLine("OTHER EXCEPTION: "+e);
        }
        //return connected
        return connected;
    }

    public static DataTable ExecuteQuery(SqlCommand command)
    {
        DataTable table = new DataTable();
        if (Open())
        {
            try
            {
                //assign connection
                command.Connection = connection;
                //adapter
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                //fill table
                adapter.Fill(table);
            }
            catch (SqlException e)
            {

            }
            //close connection
            connection.Close();
        }
        return table;
    }

    #endregion

}