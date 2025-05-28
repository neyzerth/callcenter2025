public class AgentNotFoundException : Exception
{
    public string _message;
    public override string Message => _message;

    public AgentNotFoundException(int id)
    {
        _message = "Could not find agent with the id " + id;
    }
}
