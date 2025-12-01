using System.Linq;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AkariBeauty.Data.Repositories;

public class ProfissionalRepository : GenericoRepository<Profissional>, IProfissionalRepository
{

    private readonly AppDbContext _context;
    private readonly DbSet<Profissional> _dbSet;

    public ProfissionalRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Profissional>();
    }

    public async Task<Profissional> GetByCpf(string cpf)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Cpf == cpf);
    }

    public async Task<Profissional> GetByLogin(string login)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Login == login);
    }
    public async Task<IEnumerable<Profissional>> GetByServicoId(int servicoId)
    {
        var profissionalIds = await _context.ProfissionaisServicos
            .Where(ps => ps.ServicoId == servicoId)
            .Select(ps => ps.ProfissionalId)
            .Distinct()
            .ToListAsync();

        if (!profissionalIds.Any())
            return Enumerable.Empty<Profissional>();

        var profissionais = await _context.Profissionais
            .AsNoTracking()
            .Where(p => profissionalIds.Contains(p.Id))
            .Include(p => p.ProfissionalServicos)
                .ThenInclude(ps => ps.Servico)
            .ToListAsync();

        // garante que apenas os serviços relevantes sejam serializados
        foreach (var profissional in profissionais)
        {
            profissional.ProfissionalServicos = profissional.ProfissionalServicos?
                .Where(ps => ps.ServicoId == servicoId)
                .ToList();
        }

        return profissionais;
    }
}
