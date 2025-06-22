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
}
