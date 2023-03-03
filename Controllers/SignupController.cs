using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[Controller]")]
[Route("api/[Controller]/[action]")]
[ApiController]
public class SignupController : ControllerBase
{
    private readonly DatabaseContext _context;

    public SignupController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("")]
    public IActionResult Singup([FromForm] User user)
    {
        bool status = false;
        var count = this._context.Users
            .Where(u => u.Name == user.Name)
            .Count();
        if (count == 0) {
            this._context.Users
                .Add(user);
            this._context.SaveChanges();
            status = true;
        }
        return Ok(new { status = status });
    }
}

