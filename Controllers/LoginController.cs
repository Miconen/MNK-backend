using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[Controller]")]
[Route("api/[Controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly DatabaseContext _context;

    public LoginController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("")]
    public IActionResult Login([FromForm] UserPartial user)
    {
        User query = this._context.Users.Where(u => u.Name == user.Name && u.Password == user.Password).First<User>();
        bool status = query.Password == user.Password;
        return Ok(new { status = status, res = query });
    }
}

