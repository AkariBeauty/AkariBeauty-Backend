using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;

namespace AkariBeauty.Data.Interfaces;

public interface IProfissionalServicoRepository : IGenericoRepository<ProfissionalServico>
{
    Task<ProfissionalServico> GetProfissionalAndServico(int idProfissional, int idServico);
    Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForProfissional(int idProfissional);
    Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForServico(int idServico);
}
