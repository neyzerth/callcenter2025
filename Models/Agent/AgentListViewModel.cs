public class AgentListViewModel : JsonResponse
{
    public List<Agent> Agents { get; set; }
    
    public static AgentListViewModel GetResponse(List<Agent> agents)
    {
        AgentListViewModel r = new AgentListViewModel();
        r.Status = 0;
        r.Agents = agents;
        return r;
    }
}
