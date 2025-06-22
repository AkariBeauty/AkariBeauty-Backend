using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Types;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories
{
    public class EmpresaRepository : GenericoRepository<Empresa>, IEmpresaRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Empresa> _dbSet;
        public EmpresaRepository(AppDbContext context) : base(context)
        {
            this._context = context;
            this._dbSet = _context.Set<Empresa>();
        }

        public async Task<Empresa> FindByCnpj(string cnpj)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Cnpj == cnpj);
        }

    }
}
