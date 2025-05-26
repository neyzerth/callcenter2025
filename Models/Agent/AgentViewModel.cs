public class AgentViewModel
{
    public int Status { get; set; }
    public Agent Agent { get; set; }


    public static AgentViewModel GetResponse(Agent agent)
    {
        AgentViewModel r = new AgentViewModel();
        r.Status = 0;
        r.Agent = agent;
        return r;
    }
}

