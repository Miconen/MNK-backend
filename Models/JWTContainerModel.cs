using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace backend.Models;

// Resource used: https://medium.com/@mmoshikoo/jwt-authentication-using-c-54e0c71f21b0
public class JWTContainerModel : IAuthContainerModel
{
    public int ExpireMinutes { get; set; } = 10080;
    public string SecretKey { get; set; } = "aGVsbG8gd29ybGQgaGVsbG8gd29ybGQ=";
    public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

    public Claim[] Claims { get; set; }
}
