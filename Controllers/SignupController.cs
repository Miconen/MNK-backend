using backend.Data;
using backend.Managers;
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
    public IActionResult Singup([FromForm] AuthRequest signup)
    {
        // Check if post request has correct parameters
        bool NO_NAME = string.IsNullOrWhiteSpace(signup.Name);
        bool NO_PASSWORD = string.IsNullOrWhiteSpace(signup.Password);
        if (NO_NAME || NO_PASSWORD) return Ok(new { status = "Invalid post parameters" });

        int query = this._context.Users
            .Where(u => u.Name == signup.Name)
            .Count();
        bool loggedIn = false;
        if (query == 0) {
            User user = new User(signup.Name, signup.Password);
            this._context.Users
                .Add(user);
            this._context.SaveChanges();
            loggedIn = true;
        }
        if (!loggedIn) return Ok(new { status = "Username is already taken" });

        // Generate JWT
        IAuthContainerModel model = JWTService.GetJWTContainerModel(signup.Name, signup.Password);
        IAuthService authService = new JWTService(model.SecretKey);
        string token = authService.GenerateToken(model);

        return Ok(new { status = "Success", jwt = token });
    }
}

