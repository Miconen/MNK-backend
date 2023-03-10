using backend.Data;
using backend.Managers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

using BCrypt.Net;

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
        var errors = new List<string>();

        // Check if post request has correct parameters
        if (string.IsNullOrWhiteSpace(signup.Name))
        {
            errors.Add("Name field cannot be empty");
        }
        else if (string.IsNullOrWhiteSpace(signup.Password))
        {
            errors.Add("Password field cannot be empty");
        }

        // Hash the password
        string passwordHash = "";
        try
        {
            passwordHash = BCrypt.EnhancedHashPassword(signup.Password);
        }
        catch (SaltParseException ex)
        {
            errors.Add($"Error hashing password: {ex.Message}");
        }

        // Check if name is already taken
        bool usernameTaken = this._context.Users
            .Any(u => u.Name == signup.Name);

        if (usernameTaken)
        {
            errors.Add("Username has already been taken");
        }

        // Return errors if any
        if (errors.Count > 0)
        {
            return Ok(new { status = "Error", errors = errors });
        }

        // Add user to database
        User user = new User(signup.Name!, passwordHash);
        this._context.Users.Add(user);
        this._context.SaveChanges();

        // Generate JWT
        IAuthContainerModel model = JWTService.GetJWTContainerModel(signup.Name!, "user");
        IAuthService authService = new JWTService(model.SecretKey);
        string token = authService.GenerateToken(model);

        return Ok(new { status = "Success", jwt = token });
    }
}

