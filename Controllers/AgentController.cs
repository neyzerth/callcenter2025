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
        public ActionResult Post()
        {
            return Ok("post");
        }


        [HttpGet("[action]/{station}")]
        //[Route("[action]/{station}")]
        public ActionResult SignIn(string station)
        {
            return Ok("sign in");
        }


        [HttpPost("documents/add/{document}")]
        //[Route("/documents/add/{document}")]
        public ActionResult AddDocument(string document)
        {
            return Ok("add document " + document);
        }
    }
}
