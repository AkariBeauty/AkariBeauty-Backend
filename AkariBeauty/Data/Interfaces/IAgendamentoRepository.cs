using System;
using System.Collections.Generic;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IAgendamentoRepository : IGenericoRepository<Agendamento>
    {
        Task<IEnumerable<Agendamento>> GetByClienteId(int clienteId);
        Task<IReadOnlyCollection<Agendamento>> GetByPeriodo(DateOnly inicio, DateOnly fim, int? servicoId = null, int? profissionalId = null);
    }
}
