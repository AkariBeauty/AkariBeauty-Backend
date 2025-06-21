using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Jwt;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities;

public class ProfissionalService : GenericoService<Profissional, ProfissionalDTO>, IProfissionalService
{

    private readonly IProfissionalRepository _repository;
    private readonly IProfissionalServicoRepository _profissionalServicoRepository;
    private readonly IServicoRepository _servicoService;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;

    public ProfissionalService(IProfissionalRepository repository, IServicoRepository servicoRepository, IConfiguration configuration, IProfissionalServicoRepository profissionalServicoRepository, IEmpresaRepository empresaRepository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _jwtService = new JwtService(configuration);
        _mapper = mapper;
        _profissionalServicoRepository = profissionalServicoRepository;
        _servicoService = servicoRepository;
        _empresaRepository = empresaRepository;
    }

    public async Task AddServico(ProfissionalServico request)
    {

        var servico = await _servicoService.GetById(request.ServicoId);
        var profissional = await _repository.GetById(request.ProfissionalId);
        var profissionalServico = await _profissionalServicoRepository.GetProfissionalAndServico(request.ProfissionalId, request.ServicoId);

        if (servico == null)
            throw new ArgumentException("Servico nao encontrado");

        if (profissional == null)
            throw new ArgumentException("Profissional nao encontrado");

        if (profissionalServico != null)
            throw new ArgumentException("Profissional ja possui esse servico");

        await _profissionalServicoRepository.Add(request);

    }

    public async Task<IEnumerable<ServicoDTO>> GetAllSevicos(int idProfissional)
    {
        var profissional = await _repository.GetById(idProfissional);

        if (profissional == null)
            throw new ArgumentException("Profissional nao encontrado.");

        var servicoIds = await _profissionalServicoRepository.GetProfissionalServicoForProfissional(idProfissional);

        if (servicoIds == null || !servicoIds.Any())
            throw new ArgumentException("Profissional nao possui serviços.");

        List<Servico> servicos = new List<Servico>();

        foreach (var item in servicoIds)
        {
            var servico = await _servicoService.GetById(item.ServicoId);
            servicos.Add(servico);
        }

        return _mapper.Map<IEnumerable<ServicoDTO>>(servicos);
    }
    public async Task<string> Login(RequestLoginDTO request)
    {
        Profissional user = await _repository.GetByLogin(request.Login);

        if (user == null)
            throw new ArgumentException("Usuário ou senha inválidos.");

        if (user.Senha != request.Password)
            throw new ArgumentException("Usuário ou senha inválidos.");

        return _jwtService.GenerateJwtToken(TipoUsuarioSistema.PROFISSIONAL.ToString(), user.Id.ToString());
    }

    public async Task RemoveServico(int idProfissional, int idServico)
    {
        var entity = await _profissionalServicoRepository.GetProfissionalAndServico(idProfissional, idServico);

        if (entity == null)
            throw new ArgumentException($"Esse profissional nao possui esse servico.");

        await _profissionalServicoRepository.Remove(entity);

    }

    public override async Task<ProfissionalDTO> GetById(int id)
    {
        var profissional = await _repository.GetById(id);

        if (profissional == null)
            throw new ArgumentException("Profissional nao encontrado");

        return await base.GetById(id);
    }

    public override async Task Create(Profissional entity)
    {
        if (!await Validate(entity))
            throw new ArgumentException("Profissional ja cadastrado. Cpf ou Login ja cadastrado.");

        await base.Create(entity);
    }

    public override async Task Update(Profissional entity, int id)
    {
        var profissional = await _repository.GetById(id);

        if (profissional == null)
            throw new ArgumentException("Profissional nao encontrado");

        if (entity.Id != id)
            throw new ArgumentException("Profissional nao encontrado");

        profissional = await _repository.GetByLogin(entity.Login);

        if (profissional != null && profissional.Id != id)
            throw new ArgumentException("Profissional ja cadastrado. Cpf ou Login ja cadastrado.");

        profissional = await _repository.GetByCpf(entity.Cpf);

        if (profissional != null && profissional.Id != id)
            throw new ArgumentException("Profissional ja cadastrado. Cpf ou Login ja cadastrado.");

        var empresa = await _empresaRepository.GetById(entity.EmpresaId);

        if (empresa == null)
            throw new ArgumentException("Empresa nao encontrada");

        await base.Update(entity, id);
    }

    public async Task<bool> Validate(Profissional entity)
    {

        var pro = await _repository.GetByLogin(entity.Login);

        if (pro == null)
            return false;

        pro = await _repository.GetByCpf(entity.Cpf);

        if (pro == null)
            return false;

        return true;

    }
}