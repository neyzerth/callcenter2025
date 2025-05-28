using System.Data;

public class StatusMapper
{
    public static Status ToObject(DataRow row)
    {
        int id = (int)row["id"];
        string description = (string)row["description"];
        bool availableToAnswer = (bool)row["availableToAnswer"];

        return new Status(id, description, availableToAnswer);
    }
}