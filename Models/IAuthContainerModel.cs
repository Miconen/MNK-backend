using System.Security.Claims;

namespace backend.Models;

// Resource used: https://medium.com/@mmoshikoo/jwt-authentication-using-c-54e0c71f21b0
public interface IAuthContainerModel
{
    string SecretKey { get; set; }
    string SecurityAlgorithm { get; set; }
    int ExpireMinutes { get; set; }

    Claim[] Claims { get; set; }
}
