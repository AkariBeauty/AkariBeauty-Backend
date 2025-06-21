using AkariBeauty.Objects.Relationship;

namespace AkariBeauty.Services.Interfaces;

public interface IProfissionalServicoService
{
    Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForProfissional(int profissionalId);
    Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForServico(int servicoId);
}
