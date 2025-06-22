using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Services.Interfaces;

namespace AkariBeauty.Services.Entities;

public class ProfissionalServicoService : IProfissionalServicoService
{

    private readonly IProfissionalServicoRepository _profissionalServicoRepository;

    public ProfissionalServicoService(IProfissionalServicoRepository profissionalServicoRepository)
    {
        _profissionalServicoRepository = profissionalServicoRepository;
    }

    public async Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForProfissional(int profissionalId)
    {
        var profissionalServicos = await _profissionalServicoRepository.GetProfissionalServicoForProfissional(profissionalId);

        if (profissionalServicos == null || !profissionalServicos.Any())
            throw new ArgumentException("Profissional nao possui serviços");

        return profissionalServicos;
    }

    public async Task<IEnumerable<ProfissionalServico>> GetProfissionalServicoForServico(int servicoId)
    {
        var profissionalServicos = await _profissionalServicoRepository.GetProfissionalServicoForServico(servicoId);

        if (profissionalServicos == null || !profissionalServicos.Any())
            throw new ArgumentException("Serviço nao possui profissionais");

        return profissionalServicos;
    }
}
