using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Services.Interfaces
{
    public interface IAgendamentoService : IGenericoService<Agendamento, AgendamentoDTO>
    {
        Task<IEnumerable<AgendamentoDTO>> GetAgendamentosPorData(RequestAgendamentoForDateDTO request);
    }
}
