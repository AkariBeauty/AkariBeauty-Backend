using System;
using System.Collections.Generic;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IAgendamentoRepository : IGenericoRepository<Agendamento>
    {
        Task<IEnumerable<Agendamento>> GetByClienteId(int clienteId);
        Task<IReadOnlyCollection<Agendamento>> GetByPeriodo(DateOnly inicio, DateOnly fim, int? servicoId = null, int? profissionalId = null);
        Task<IReadOnlyCollection<Agendamento>> GetAgendaProfissionalAsync(int profissionalId, DateOnly inicio, DateOnly fim);
        Task<Agendamento?> GetDetalheProfissionalAsync(int profissionalId, int agendamentoId);
        Task<int> CountAgendamentosAsync(int profissionalId, DateOnly inicio, DateOnly fim, StatusAgendamento? status = null);
    }
}
