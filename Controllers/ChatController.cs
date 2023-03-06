using backend.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace backend.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hub;

    public ChatController(IHubContext<ChatHub> hub)
    {
        _hub = hub;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _hub.Clients.All.SendAsync("ChatMessage", "Hello world");
        return Ok(new { Message = "Request completed" });
    }
}

