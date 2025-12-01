using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Objects.Dtos.Profissionais;

namespace AkariBeauty.Services.Interfaces;

public interface IProfissionalService : IGenericoService<Profissional, ProfissionalDTO>, IGenericLogin
{
    Task AddServico(ProfissionalServico request);
    Task RemoveServico(int idProfissional, int idServico);
    Task <IEnumerable<ServicoDTO>> GetAllSevicos(int idProfissional);
    Task<IEnumerable<ProfissionalComServicosDTO>> GetByServicoId(int servicoId);
}
