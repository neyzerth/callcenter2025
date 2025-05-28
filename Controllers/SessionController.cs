using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SessionController : ControllerBase
{
    [HttpGet("[action]/{station}")]
    //[Route("[action]/(station)")]
    public ActionResult Login(int station)
    {
        if (!String.IsNullOrEmpty(Request.Headers["agent"])
            && !String.IsNullOrEmpty(Request.Headers["pin"]))
        {
            //headers 
            int agentId = Int32.Parse(Request.Headers["agent"]);
            int pin = Int32.Parse(Request.Headers["pin"]);
            int result = Session.Login(agentId, pin, station);
            
            //message type
            // MessageType type = MessageType.Success;
            // if (result > 0) type = MessageType.Error;
            MessageType type = result > 0 ? MessageType.Error : MessageType.Success;
            
            //login result
            string message = ((LoginResult)result).ToString();
            //response
            return Ok(MessageResponse.GetResponse(result, message, type));    
        }


        return Ok(MessageResponse.GetResponse(500, "Missing Security Headers", MessageType.Error));
    }
}