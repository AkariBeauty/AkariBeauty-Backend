using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Repositories
{
    public class AgendamentoRepository : GenericoRepository<Agendamento>, IAgendamentoRepository
    {
        private readonly AppDbContext _context;

        public AgendamentoRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
