using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

    namespace CallCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            List<Agent> list = Agent.Get();

            return Ok(AgentListViewModel.GetResponse(list));
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                Agent a = Agent.Get(id);
                return Ok(AgentViewModel.GetResponse(a));
            }
            catch (AgentNotFoundException e)
            {
                return Ok(MessageResponse.GetResponse(1, e.Message, MessageType.Error));
            }
            catch (Exception e)
            {
                return Ok(MessageResponse.GetResponse(999, e.Message, MessageType.Error));
            }

        }

        [HttpPost]
        [Route("")]
        public ActionResult Post([FromForm] PostAgent p)
        {
            bool result = Agent.Insert(p);
            if(result)
                return Ok(MessageResponse.GetResponse(1, "Post agent successful", MessageType.Success));
            
            return Ok(MessageResponse.GetResponse(999, "Error posting agent", MessageType.Error));
        }

        [HttpPost("documents/add/{document}")]
        //[Route("/documents/add/{document}")]
        public ActionResult AddDocument(string document)
        {
            return Ok("add document " + document);
        }
    }
}
