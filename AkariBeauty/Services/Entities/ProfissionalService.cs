using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AkariBeauty.Authentication;               // JwtService, TokenPayload
using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Dtos.Profissionais;
using AkariBeauty.Objects.Enums;               // TipoUsuario
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AkariBeauty.Services.Interfaces;

namespace AkariBeauty.Services.Entities
{
    public class ProfissionalService : GenericoService<Profissional, ProfissionalDTO>, IProfissionalService
    {
        private readonly IProfissionalRepository _repository;
        private readonly IProfissionalServicoRepository _profissionalServicoRepository;
        private readonly IServicoRepository _servicoService;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public ProfissionalService(
            IProfissionalRepository repository,
            IServicoRepository servicoRepository,
            IProfissionalServicoRepository profissionalServicoRepository,
            IEmpresaRepository empresaRepository,
            JwtService jwtService,                      // ✅ injeta via DI
            IMapper mapper
        ) : base(repository, mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _servicoService = servicoRepository ?? throw new ArgumentNullException(nameof(servicoRepository));
            _profissionalServicoRepository = profissionalServicoRepository ?? throw new ArgumentNullException(nameof(profissionalServicoRepository));
            _empresaRepository = empresaRepository ?? throw new ArgumentNullException(nameof(empresaRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddServico(ProfissionalServico request)
        {
            var servico = await _servicoService.GetById(request.ServicoId);
            var profissional = await _repository.GetById(request.ProfissionalId);
            var profissionalServico = await _profissionalServicoRepository.GetProfissionalAndServico(request.ProfissionalId, request.ServicoId);

            if (servico == null)
                throw new ArgumentException("Servico nao encontrado.");

            if (profissional == null)
                throw new ArgumentException("Profissional nao encontrado.");

            if (profissionalServico != null)
                throw new ArgumentException("Profissional ja possui esse servico.");

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

            var servicos = new List<Servico>();
            foreach (var item in servicoIds)
            {
                var servico = await _servicoService.GetById(item.ServicoId);
                if (servico != null) servicos.Add(servico);
            }

            return _mapper.Map<IEnumerable<ServicoDTO>>(servicos);
        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request?.Login) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Usuário ou senha inválidos.");

            var user = await _repository.GetByLogin(request.Login);
            if (user == null)
                throw new ArgumentException("Usuário ou senha inválidos.");

            if (user.Senha != request.Password) 
                throw new ArgumentException("Usuário ou senha inválidos.");

            var payload = new TokenPayload(
                UserId: user.Id.ToString(),
                Role: "PROFISSIONAL", 
                Name: user.Nome,
                Email: user.Login,
                EmpresaId: (user.EmpresaId > 0) ? user.EmpresaId.ToString() : null

            );

            return _jwtService.CreateAccessToken(payload);
        }

        public async Task RemoveServico(int idProfissional, int idServico)
        {
            var entity = await _profissionalServicoRepository.GetProfissionalAndServico(idProfissional, idServico);
            if (entity == null)
                throw new ArgumentException("Esse profissional nao possui esse servico.");

            await _profissionalServicoRepository.Remove(entity);
        }

        public async Task<IEnumerable<ProfissionalComServicosDTO>> GetByServicoId(int servicoId)
        {
            if (servicoId <= 0)
                throw new ArgumentException("Serviço inválido.");

            var profissionais = await _repository.GetByServicoId(servicoId);
            return profissionais.Select(profissional => new ProfissionalComServicosDTO
            {
                Id = profissional.Id,
                Nome = profissional.Nome,
                Telefone = profissional.Telefone,
                ProfissionalServicos = profissional.ProfissionalServicos?.Select(ps => new ProfissionalComServicosRelacaoDTO
                {
                    ServicoId = ps.ServicoId,
                    Servico = ps.Servico == null ? null : new ServicoNomeDTO { ServicoPrestado = ps.Servico.ServicoPrestado }
                }) ?? Enumerable.Empty<ProfissionalComServicosRelacaoDTO>()
            });
        }

        public override async Task<ProfissionalDTO> GetById(int id)
        {
            var profissional = await _repository.GetById(id);
            if (profissional == null)
                throw new ArgumentException("Profissional nao encontrado.");

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
                throw new ArgumentException("Profissional nao encontrado.");

            if (entity.Id != id)
                throw new ArgumentException("Profissional nao encontrado.");

            // garante unicidade de login
            var porLogin = await _repository.GetByLogin(entity.Login);
            if (porLogin != null && porLogin.Id != id)
                throw new ArgumentException("Profissional ja cadastrado. Cpf ou Login ja cadastrado.");

            // garante unicidade de cpf
            var porCpf = await _repository.GetByCpf(entity.Cpf);
            if (porCpf != null && porCpf.Id != id)
                throw new ArgumentException("Profissional ja cadastrado. Cpf ou Login ja cadastrado.");

            var empresa = await _empresaRepository.GetById(entity.EmpresaId);
            if (empresa == null)
                throw new ArgumentException("Empresa nao encontrada.");

            await base.Update(entity, id);
        }

        public async Task<bool> Validate(Profissional entity)
        {
            // válido quando NÃO houver duplicidade de login/cpf
            var porLogin = await _repository.GetByLogin(entity.Login);
            if (porLogin != null) return false;

            var porCpf = await _repository.GetByCpf(entity.Cpf);
            if (porCpf != null) return false;

            return true;
        }

        private static string MapTipoUsuarioToRole(TipoUsuario tipo) => tipo switch
        {
            TipoUsuario.PROFISSIONAL => "PROFISSIONAL",
            TipoUsuario.ADMIN => "ADMIN",
            TipoUsuario.RECEPCIONISTA => "RECEPCIONISTA",
            _ => "USUARIO"
        };
    }
}
