using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AkariBeauty.Jwt;

public class JwtService : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateJwtToken(InfoToken infoToken)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var type = infoToken.Tipo;
        var identifier = infoToken.Id;

        var claims = new[]
        {
            new Claim("type", type.ToString()),
            new Claim("identifier", identifier.ToString())
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

    public InfoToken GetInfoToken()
    {
        string token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];

        token = ValidateToken(token);

        var header = new JwtSecurityTokenHandler();

        var jwtToken = header.ReadJwtToken(token);

        var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

        var type = Enum.Parse<TipoUsuarioSistema>(claims["type"]);
        int identifier = int.Parse(claims["identifier"]);

        var infoToken = new InfoToken();

        infoToken.Tipo = type;
        infoToken.Id = identifier;

        return infoToken;
    }

    public string ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            throw new ArgumentException("Token não fornecido ou inválido.");

        var returnToken = token.Replace("Bearer ", "");
        
        return returnToken;
    }
}
