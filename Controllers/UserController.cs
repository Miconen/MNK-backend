using backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[Controller]")]
[Route("api/[Controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _context;

    public UserController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Default()
    {
        var find = this._context.Users.Find(1);
        return Ok(new { Message = find });
    }

    [HttpGet]
    [Route("{userId:int}")]
    public IActionResult ById(int userId)
    {
        var user = this._context.Users.Find(userId);
        if (user is null)
        {
            return Ok(new { error = "Couldn't find user with id: " + userId });
        }
        return Ok(new { user = user });
    }
}

