using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using Microsoft.EntityFrameworkCore;

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
                .Include(a => a.Profissional)
                .Include(a => a.Servicos)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Agendamento>> GetByPeriodo(DateOnly inicio, DateOnly fim, int? servicoId = null, int? profissionalId = null)
        {
            var query = _context.Agendamentos
                .Include(a => a.Servicos)
                .Include(a => a.Profissional)
                .Where(a => a.Data >= inicio && a.Data <= fim)
                .AsQueryable();

            if (servicoId.HasValue)
            {
                query = query.Where(a => a.Servicos.Any(s => s.Id == servicoId.Value));
            }

            if (profissionalId.HasValue)
            {
                query = query.Where(a => a.ProfissionalId == profissionalId.Value);
            }

            var itens = await query.ToListAsync();
            return itens;
        }
    }
}
