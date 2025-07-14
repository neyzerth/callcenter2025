using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CallController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public ActionResult Get()
    {
        List<Call> list = Call.Get();

        return Ok(CallListViewModel.GetResponse(list));
    }

    // [HttpGet]
    // [Route("{id}")]
    // public ActionResult Get(int id)
    // {
    //     try
    //     {
    //         Call a = Call.Get(id);
    //         return Ok(CallViewModel.GetResponse(a));
    //     }
    //     catch (Exception e)
    //     {
    //         return Ok(MessageResponse.GetResponse(999, e.Message, MessageType.Error));
    //     }
    //
    // }
    
    [HttpPost("[action]/{phoneNumber}")]
    public ActionResult Receive(string phoneNumber)
    {
        int result = Call.ReceiveCall(phoneNumber);
        
        //message type
        // MessageType type = MessageType.Success;
        // if (result > 0) type = MessageType.Error;
        MessageType type = result > 0 ? MessageType.Error : MessageType.Success;
        
        //login result
        string message = result > 0 ? "Error: Call not received" : "Call received" ;
        
        //response
        return Ok(MessageResponse.GetResponse(result, message, type)); 
    }
    [HttpGet("[action]/{callId}/{statusEnd}")]
    public ActionResult End(int callId, int statusEnd)
    {
        int result = Call.EndCall(callId, statusEnd);
        
        //message type
        // MessageType type = MessageType.Success;
        // if (result > 0) type = MessageType.Error;
        MessageType type = result > 0 ? MessageType.Error : MessageType.Success;
        
        //login result
        string message = ((EndCallResult)result).ToString();
        
        //response
        return Ok(MessageResponse.GetResponse(result, message, type)); 
    }
    [HttpGet("[action]")]
    public ActionResult EndRandom()
    {
        
        int result = Call.EndCallRandom();
        
        
        //login result
        string message = ((EndCallResult)result).ToString();
        
        //response
        return Ok(MessageResponse.GetResponse(0, "Calls ended "+ result, MessageType.Information)); 
    }
    
    [HttpGet("[action]/{date}")]
    public ActionResult Totals(DateTime date)
    {
        return Ok(ViewTotalsResponse.GetResponse(date));
    }
}