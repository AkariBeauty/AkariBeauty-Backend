using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AkariBeauty.Data.Repositories
{
    public class AgendamentoRepository : GenericoRepository<Agendamento>, IAgendamentoRepository
    {
        private readonly AppDbContext _context;

        public AgendamentoRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Agendamento>> GetByClienteId(int clienteId)
        {
            return await _context.Agendamentos
                .Where(a => a.ClienteId == clienteId)
                .Include(a => a.Servicos)
                .ToListAsync();
        }
    }
}
