using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Managers;

// Resource used: https://medium.com/@mmoshikoo/jwt-authentication-using-c-54e0c71f21b0
public class JWTService : IAuthService
{
    public string SecretKey { get; set; }

    public JWTService(string secretKey)
    {
        SecretKey = secretKey;
    }

    public bool IsTokenValid(string token)
    {

        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Given token is null or empty.");

        TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GenerateToken(IAuthContainerModel model)
    {
        if (model == null || model.Claims == null || model.Claims.Length == 0)
        {
            throw new ArgumentException("Arguments to create token are not valid.");
        }

        SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(model.Claims),
            Expires = DateTime.UtcNow.AddMinutes(model.ExpireMinutes),
            SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        string token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return token;
    }

    public IEnumerable<Claim> GetTokenClaims(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Given token is null or empty.");

        TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        return tokenValid.Claims;
    }

    public static JWTContainerModel GetJWTContainerModel(string name, string role)
    {
        return new JWTContainerModel()
        {
            Claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            }
        };
    }

    private SecurityKey GetSymmetricSecurityKey()
    {
        byte[] symmetricKey = Convert.FromBase64String(SecretKey);
        return new SymmetricSecurityKey(symmetricKey);
    }

    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = GetSymmetricSecurityKey()
        };
    }
}
