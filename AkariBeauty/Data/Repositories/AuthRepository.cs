/*using AkariBeauty.Data.Interfaces;
using AkariBeauty.Data.Interfaces.Repositories;

namespace AkariBeauty.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }cyy

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            // Valide as credenciais do usuário no banco de dados
            var user = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            // Obtenha o papel (role) do usuário
            var user = await _context.Usuario.FirstOrDefaultAsync(u => u.Username == username);
            return user?.Role;
        }
    }
}*/