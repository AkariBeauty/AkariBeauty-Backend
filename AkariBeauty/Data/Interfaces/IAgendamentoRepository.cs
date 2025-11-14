using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IAgendamentoRepository : IGenericoRepository<Agendamento>
    {
        Task<IEnumerable<Agendamento>> GetByClienteId(int clienteId);
    }
}
