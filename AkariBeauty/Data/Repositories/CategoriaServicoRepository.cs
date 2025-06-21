using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Repositories;

public class CategoriaServicoRepository : GenericoRepository<CategoriaServico>, ICategoriaServicoRepository
{
    private readonly AppDbContext _context;
    public CategoriaServicoRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
