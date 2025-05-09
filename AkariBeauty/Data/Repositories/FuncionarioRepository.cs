using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Repositories
{
    public class FuncionarioRepository : GenericoRepository<Funcionario>, IFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
