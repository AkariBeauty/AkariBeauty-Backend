using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AkariBeauty.Authentication
{
    /// <summary>
    /// Serviço responsável por criar e ler JWTs.
    /// </summary>
    public sealed class JwtService
    {
        private readonly JwtSettings _settings;
        private readonly JwtSecurityTokenHandler _handler = new();

        public JwtService(IOptions<JwtSettings> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(_settings.Key))
                throw new ArgumentException("JwtSettings.Key não configurado.");
        }

        /// <summary>
        /// Cria um token de acesso com as claims fornecidas.
        /// </summary>
        public string CreateAccessToken(TokenPayload payload)
        {
            if (payload is null) throw new ArgumentNullException(nameof(payload));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, payload.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, NormalizeRole(payload.Role))
            };

            if (!string.IsNullOrWhiteSpace(payload.Name))
                claims.Add(new Claim(ClaimTypes.Name, payload.Name));

            if (!string.IsNullOrWhiteSpace(payload.Email))
                claims.Add(new Claim(ClaimTypes.Email, payload.Email));

            if (!string.IsNullOrWhiteSpace(payload.ClienteId))
                claims.Add(new Claim("clienteId", payload.ClienteId));

            if (!string.IsNullOrWhiteSpace(payload.EmpresaId))
                claims.Add(new Claim("empresaId", payload.EmpresaId));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresMinutes = _settings.ExpireMinutes > 0 ? _settings.ExpireMinutes : 60;

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return _handler.WriteToken(token);
        }

        /// <summary>
        /// Lê todas as claims de um token (aceita "Bearer x.y.z").
        /// </summary>
        public Dictionary<string, string> GetInfoToken(string token)
        {
            token = StripBearer(token);
            var jwt = _handler.ReadJwtToken(token);

            return jwt.Claims.ToDictionary(c => c.Type, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Remove o prefixo "Bearer " se presente.
        /// </summary>
        public static string StripBearer(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token não fornecido ou inválido.");

            return token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? token.Substring("Bearer ".Length)
                : token;
        }

        private static bool IsCliente(string role) =>
            string.Equals(role, "CLIENTE", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(role, "Cliente", StringComparison.OrdinalIgnoreCase);

        private static string NormalizeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return role ?? string.Empty;
            if (IsCliente(role)) return "Cliente";
            if (string.Equals(role, "ADMIN", StringComparison.OrdinalIgnoreCase)) return "Admin";
            if (string.Equals(role, "PROFISSIONAL", StringComparison.OrdinalIgnoreCase)) return "Profissional";
            if (string.Equals(role, "RECEPCIONISTA", StringComparison.OrdinalIgnoreCase)) return "Recepcionista";
            return role;
        }
    }

    /// <summary>
    /// Payload para criação do token.
    /// </summary>
    public sealed record TokenPayload(
        string UserId,
        string Role,
        string? Name = null,
        string? Email = null,
        string? ClienteId = null,
        string? EmpresaId = null
    );
}
