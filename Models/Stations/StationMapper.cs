using System.Data;

public class StationMapper
{
    public static Station ToObject(DataRow row)
    {
        //cast columns
        int id = (int)row["id"];
        int rowNumber = (int)row["rowNumber"];
        int deskNumber = (int)row["deskNumber"];
        string ipAddress = (string)row["ipAddress"];
        bool active = (bool)row["active"];
        
        return new Station(id, rowNumber, deskNumber, ipAddress, active);
    }

    public static List<Station> ToList(DataTable table)
    {
        //create a list
        List<Station> list = new List<Station>();
        
        //populate list
        foreach (DataRow row in table.Rows)
        { 
            list.Add(ToObject(row));
        }

        return list;
    }
}