using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories
{
    public class UsuarioRepository : GenericoRepository<Usuario>, IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public Task<Usuario> GetByLogin(string login)
        {
            DbSet<Usuario> _dbSet = _context.Set<Usuario>();
            return _dbSet.FirstOrDefaultAsync(e => e.Login == login);
        }
    }
}
