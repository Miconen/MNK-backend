using System.Security.Claims;
using System.Collections.Generic;
using backend.Models;

namespace backend.Managers;

public interface IAuthService
{
    string SecretKey { get; set; }
    bool IsTokenValid(string token);
    string GenerateToken(IAuthContainerModel model);
    IEnumerable<Claim> GetTokenClaims(string token);
}
