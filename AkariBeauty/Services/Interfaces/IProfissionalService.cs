using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;

namespace AkariBeauty.Services.Interfaces;

public interface IProfissionalService : IGenericoService<Profissional, ProfissionalDTO>, IGenericLogin
{
    Task AddServico(ProfissionalServico request);
    Task RemoveServico(int idProfissional, int idServico);
    Task <IEnumerable<ServicoDTO>> GetAllSevicos(int idProfissional);
}
