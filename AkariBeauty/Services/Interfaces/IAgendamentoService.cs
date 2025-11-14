using AkariBeauty.Objects.Dtos.Agendamentos;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Services.Interfaces
{
    public interface IAgendamentoService : IGenericoService<Agendamento, AgendamentoDTO>
    {
        Task<IEnumerable<AgendamentoDetalheDTO>> GetByClienteId(int clienteId);
        Task<AgendamentoDetalheDTO> CreateAgendamentoAsync(CriarAgendamentoRequest request);
    }
}
