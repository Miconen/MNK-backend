using backend.Data;
using backend.Managers;
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
    public IActionResult Login([FromForm] AuthRequest login)
    {
        // Check if post request has correct parameters
        bool NO_NAME = string.IsNullOrWhiteSpace(login.Name);
        bool NO_PASSWORD = string.IsNullOrWhiteSpace(login.Password);
        if (NO_NAME || NO_PASSWORD) return Ok(new { status = "Invalid post parameters" });

        // Check claims
        /* if (!authService.IsTokenValid(token)) */
        /*     throw new UnauthorizedAccessException(); */
        /* else */
        /* { */
        /*     List<Claim> claims = authService.GetTokenClaims(token).ToList(); */
        /*     Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value); */
        /* } */

        User? query = this._context.Users.Where(user => user.Name == login.Name && user.Password == login.Password).FirstOrDefault<User>();
        bool loggedIn;
        try
        {
            loggedIn = query.Password == login.Password;
        }
        catch (NullReferenceException)
        {
            loggedIn = false;
        }
        if (!loggedIn) return Ok(new { status = "Invalid credentials" });

        // Generate JWT
        IAuthContainerModel model = JWTService.GetJWTContainerModel(query.Name, query.Role);
        IAuthService authService = new JWTService(model.SecretKey);
        string token = authService.GenerateToken(model);

        return Ok(new { status = "Success", jwt = token });
    }
}

