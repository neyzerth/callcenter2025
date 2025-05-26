public class StationNotFoundException : Exception
{
    public string _message;
    public override string Message => _message;
    
    public StationNotFoundException(int id)
    {
        _message = "Could not find station with the id " + id;
    }
}