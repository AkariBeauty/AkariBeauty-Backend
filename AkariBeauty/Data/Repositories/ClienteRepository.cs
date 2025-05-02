using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Repositories
{
    public class ClienteRepository : GenericoRepository<Cliente>, IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
