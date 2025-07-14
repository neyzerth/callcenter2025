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
    
    #endregion

    #region class methods

    private static SqlConnection GetConnection()
    {
        //connection
        SqlConnection connection = new SqlConnection();
        //open
        try
        {
            //connection
            connection.ConnectionString = connectionString;
            //open
            connection.Open();
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
        return connection;
    }
    
    
    public static DataTable ExecuteQuery(SqlCommand command)
    {
        DataTable table = new DataTable();
        //get the connection to DB server
        SqlConnection connection = GetConnection();
        
        if (connection.State == ConnectionState.Open)
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
                Console.WriteLine("SQL EXCEPTION: "+e);
            }
            catch (Exception e)
            {
                Console.WriteLine("AN EXCEPTION: "+e);
            }
            //close connection
            connection.Close();
        }
        return table;
    }
    //<summary>
    //Execute a stored procedure
    //</summary>
    public static int ExecuteProcedure(SqlCommand command)
    {
        //result
        int result = 0;
        //get the connection to DB server
        SqlConnection connection = GetConnection();
        
        if (connection.State == ConnectionState.Open)
        {
            try
            {
                //assign connection
                command.Connection = connection;
                //command is a store procedure
                command.CommandType = CommandType.StoredProcedure;
                //result parameter
                SqlParameter resultParameter = new SqlParameter("@status", DbType.Int32);
                //parameter is output
                resultParameter.Direction = ParameterDirection.Output;
                //add parameter to command
                command.Parameters.Add(resultParameter);
                //execute procedure
                command.ExecuteNonQuery();
                //result
                result =(int) command.Parameters["@status"].Value;

            }
            catch (SqlException e)
            {
                Console.WriteLine("SQL EXCEPTION: "+e);
            }
            catch (Exception e)
            {
                Console.WriteLine("AN EXCEPTION: "+e);
            }
            //close connection
            connection.Close();
        }
        return result;
    }

    //<summary>
    //Execute a stored procedure
    //</summary>
    public static bool ExecuteCommand(SqlCommand command)
    {
        //result
        bool result = false;
        //get the connection to DB server
        SqlConnection connection = GetConnection();
        
        if (connection.State == ConnectionState.Open)
        {
            try
            {
                //assign connection
                command.Connection = connection;
                //execute procedure
                command.ExecuteNonQuery();
                //success
                result = true;

            }
            catch (SqlException e)
            { Console.WriteLine("SQL EXCEPTION: "+e); }
            catch (Exception e)
            { Console.WriteLine("AN EXCEPTION: "+e); }
            //close connection
            connection.Close();
        }
        return result;
    }

    #endregion

}