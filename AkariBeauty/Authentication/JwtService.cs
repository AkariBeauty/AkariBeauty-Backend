using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AkariBeauty.Objects.Enums;
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("type", type.ToString()),
            new Claim("identifier", identifier)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            // expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
            expires: DateTime.MaxValue,
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
