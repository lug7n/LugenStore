using LugenStore.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LugenStore.API.Services.Security.Token;

public class TokenService(IConfiguration config) : ITokenService
{

    public string GenerateToken(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var jwtKey = config["Jwt:Key"];

        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("Configuration value 'Jwt:Key' is missing or empty.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: "LugenStore",
            audience: "LugenStoreUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
