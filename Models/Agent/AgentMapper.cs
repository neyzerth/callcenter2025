using System.Data;

public class AgentMapper
{
    public static Agent ToObject(DataRow row)
    {
        //cast columns
        int id = (int)row["agent_id"];
        string name = (string)row["agent_name"];
        string photo = (string)row["agent_photo"];
        int pin = (int)row["agent_pin"];
        
        return new Agent(id, name, photo, pin);
    }

    public static List<Agent> ToList(DataTable table)
    {
        //create a list
        List<Agent> list = new List<Agent>();
        
        //populate list
        foreach (DataRow row in table.Rows)
        { 
            list.Add(ToObject(row));
        }

        return list;
    }
}