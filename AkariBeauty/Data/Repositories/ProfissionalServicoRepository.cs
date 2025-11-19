using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories;

public class ProfissionalServicoRepository : GenericoRepository<ProfissionalServico>, IProfissionalServicoRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<ProfissionalServico> _dbSet;
    public ProfissionalServicoRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<ProfissionalServico>();
    }

    public async Task<ProfissionalServico> GetProfissionalAndServico(int idProfissional, int idServico)
    {
        return await _dbSet
            .Include(e => e.Profissional)
            .Include(e => e.Servico)
            .FirstOrDefaultAsync(e => e.ProfissionalId == idProfissional && e.ServicoId == idServico) ?? null;
    } 

    public async Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForProfissional(int idProfissional)
    {
        return await _dbSet.Where(e => e.ProfissionalId == idProfissional).ToListAsync();
    }

    public async Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForServico(int idServico)
    {
        return await _dbSet.Where(e => e.ServicoId == idServico).ToListAsync();
    }
}
