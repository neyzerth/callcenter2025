using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StationController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public ActionResult Get()
    {
        List<Station> list = Station.Get();
        
        return Ok(StationListViewModel.GetResponse(list));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult Get(int id)
    {
        try
        {
            Station s = Station.Get(id);
            return Ok(StationViewModel.GetResponse(s));
        }
        catch (StationNotFoundException e)
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
}