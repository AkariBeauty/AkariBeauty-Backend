using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Enums;
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
                .Include(a => a.Cliente)
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

        public async Task<IReadOnlyCollection<Agendamento>> GetAgendaProfissionalAsync(int profissionalId, DateOnly inicio, DateOnly fim)
        {
            return await _context.Agendamentos
                .Include(a => a.Servicos)
                .Include(a => a.Cliente)
                .Where(a => a.ProfissionalId == profissionalId && a.Data >= inicio && a.Data <= fim)
                .OrderBy(a => a.Data)
                .ThenBy(a => a.Hora)
                .ToListAsync();
        }

        public async Task<Agendamento?> GetDetalheProfissionalAsync(int profissionalId, int agendamentoId)
        {
            return await _context.Agendamentos
                .Include(a => a.Servicos)
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .FirstOrDefaultAsync(a => a.Id == agendamentoId && a.ProfissionalId == profissionalId);
        }

        public async Task<int> CountAgendamentosAsync(int profissionalId, DateOnly inicio, DateOnly fim, StatusAgendamento? status = null)
        {
            var query = _context.Agendamentos
                .Where(a => a.ProfissionalId == profissionalId && a.Data >= inicio && a.Data <= fim);

            if (status.HasValue)
            {
                query = query.Where(a => a.StatusAgendamento == status.Value);
            }

            return await query.CountAsync();
        }
    }
}
