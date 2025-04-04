using AkariBeauty.Authentication;
using AkariBeauty.Data.Interfaces;

namespace AkariBeauty.Services
{
    public class AuthService : IAuthRepository
    {
        private readonly JwtHelper _jwtHelper;

        public AuthService(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        public string Authenticate(string username, string password)
        {
            // Validar credenciais (simulação ou banco de dados)
            if (username == "admin" && password == "password")
            {
                return _jwtHelper.GenerateToken(username, "Admin");
            }

            return null; // Credenciais inválidas
        }
    }
}
