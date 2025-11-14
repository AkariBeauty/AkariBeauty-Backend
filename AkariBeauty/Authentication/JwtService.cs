using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AkariBeauty.Jwt;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(string type, string identifier)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var keyValue = jwtSettings["Key"] ?? throw new InvalidOperationException("JwtSettings:Key não configurado.");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JwtSettings:Issuer não configurado.");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JwtSettings:Audience não configurado.");
        var expireConfig = jwtSettings["ExpireMinutes"] ?? throw new InvalidOperationException("JwtSettings:ExpireMinutes não configurado.");

        if (!double.TryParse(expireConfig, out var expireMinutes))
            throw new InvalidOperationException("JwtSettings:ExpireMinutes inválido.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("type", type.ToString()),
            new Claim("identifier", identifier)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Dictionary<string, string> GetInfoToken(string token)
    {
        token = ValidateToken(token);

        var header = new JwtSecurityTokenHandler();

        var jwtToken = header.ReadJwtToken(token);

        var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

        return claims;
    }

    public string ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            throw new ArgumentException("Token não fornecido ou inválido.");

        var returnToken = token.Replace("Bearer ", "");
        
        return returnToken;
    }
}
