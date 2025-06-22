using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories
{
    public class ClienteRepository : GenericoRepository<Cliente>, IClienteRepository
    {
        private readonly AppDbContext _context;

        private readonly DbSet<Cliente> _dbSet;

        public ClienteRepository(AppDbContext context) : base(context)
        {
            this._context = context;
            this._dbSet = _context.Set<Cliente>();
        }

        public async Task<Cliente> GetByLogin(string login)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Login == login);
        }
    }
}
    