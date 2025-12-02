using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AkariBeauty.Authentication;               // JwtService, TokenPayload
using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Agendamentos;
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
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public ProfissionalService(
            IProfissionalRepository repository,
            IServicoRepository servicoRepository,
            IProfissionalServicoRepository profissionalServicoRepository,
            IEmpresaRepository empresaRepository,
            IAgendamentoRepository agendamentoRepository,
            JwtService jwtService,                      // ✅ injeta via DI
            IMapper mapper
        ) : base(repository, mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _servicoService = servicoRepository ?? throw new ArgumentNullException(nameof(servicoRepository));
            _profissionalServicoRepository = profissionalServicoRepository ?? throw new ArgumentNullException(nameof(profissionalServicoRepository));
            _empresaRepository = empresaRepository ?? throw new ArgumentNullException(nameof(empresaRepository));
            _agendamentoRepository = agendamentoRepository ?? throw new ArgumentNullException(nameof(agendamentoRepository));
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

        public async Task<ProfissionalDashboardDTO> GetDashboardAsync(int profissionalId)
        {
            var profissional = await _repository.GetById(profissionalId)
                ?? throw new ArgumentException("Profissional nao encontrado.");

            var hoje = DateOnly.FromDateTime(DateTime.Now);
            var semanaInicio = GetWeekStart(hoje);
            var semanaFim = semanaInicio.AddDays(6);

            var agendamentosSemana = await _agendamentoRepository.GetAgendaProfissionalAsync(profissionalId, semanaInicio, semanaFim);
            var agora = DateTime.Now;

            var pendentesHoje = agendamentosSemana.Count(a => a.Data == hoje && a.StatusAgendamento == StatusAgendamento.PENDENTE);
            var confirmadosHoje = agendamentosSemana.Count(a => a.Data == hoje && a.StatusAgendamento == StatusAgendamento.CONFIRMADO);
            var totalSemana = agendamentosSemana.Count(a => a.StatusAgendamento != StatusAgendamento.CANCELADO && a.StatusAgendamento != StatusAgendamento.CANCELADO_EMPRESA);
            var canceladosSemana = agendamentosSemana.Count(a => a.StatusAgendamento == StatusAgendamento.CANCELADO || a.StatusAgendamento == StatusAgendamento.CANCELADO_EMPRESA);

            var proximos = agendamentosSemana
                .Where(a => a.Data.ToDateTime(a.Hora) >= agora)
                .OrderBy(a => a.Data)
                .ThenBy(a => a.Hora)
                .Take(5)
                .Select(MapToAgendaItem)
                .ToList();

            return new ProfissionalDashboardDTO
            {
                Nome = profissional.Nome,
                PendentesHoje = pendentesHoje,
                ConfirmadosHoje = confirmadosHoje,
                TotalSemana = totalSemana,
                CanceladosSemana = canceladosSemana,
                Proximos = proximos
            };
        }

        public async Task<ProfissionalAgendaDiaDTO> GetAgendaDiaAsync(int profissionalId, DateOnly data)
        {
            var itens = await _agendamentoRepository.GetAgendaProfissionalAsync(profissionalId, data, data);
            return new ProfissionalAgendaDiaDTO
            {
                Data = data,
                Agendamentos = itens.OrderBy(a => a.Hora).Select(MapToAgendaItem)
            };
        }

        public async Task<IEnumerable<ProfissionalAgendaDiaDTO>> GetAgendaSemanaAsync(int profissionalId, DateOnly inicioSemana)
        {
            var start = GetWeekStart(inicioSemana);
            var end = start.AddDays(6);
            var itens = await _agendamentoRepository.GetAgendaProfissionalAsync(profissionalId, start, end);

            return itens
                .GroupBy(a => a.Data)
                .OrderBy(g => g.Key)
                .Select(g => new ProfissionalAgendaDiaDTO
                {
                    Data = g.Key,
                    Agendamentos = g.OrderBy(x => x.Hora).Select(MapToAgendaItem)
                });
        }

        public async Task<ProfissionalAgendamentoDetalheDTO> GetAgendamentoDetalheAsync(int profissionalId, int agendamentoId)
        {
            var agendamento = await _agendamentoRepository.GetDetalheProfissionalAsync(profissionalId, agendamentoId)
                ?? throw new ArgumentException("Agendamento nao encontrado para este profissional.");

            return MapToDetalheDto(agendamento);
        }

        public async Task AtualizarStatusAgendamentoAsync(int profissionalId, int agendamentoId, AtualizarStatusAgendamentoDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!System.Enum.IsDefined(typeof(StatusAgendamento), request.NovoStatus))
                throw new ArgumentException("Status informado é inválido.");

            var agendamento = await _agendamentoRepository.GetDetalheProfissionalAsync(profissionalId, agendamentoId)
                ?? throw new ArgumentException("Agendamento nao encontrado para este profissional.");

            if (!PodeTransicionar(agendamento.StatusAgendamento, request.NovoStatus))
                throw new ArgumentException("Não é possível atualizar o status para o valor informado.");

            agendamento.StatusAgendamento = request.NovoStatus;

            if (!string.IsNullOrWhiteSpace(request.Justificativa))
            {
                var anotacao = request.Justificativa!.Trim();
                agendamento.Observacao = string.IsNullOrWhiteSpace(agendamento.Observacao)
                    ? anotacao
                    : $"{agendamento.Observacao}\n{DateTime.Now:dd/MM HH:mm} - {anotacao}";
            }

            await _agendamentoRepository.SaveChanges();
        }

        public async Task<ProfissionalPerfilDTO> GetPerfilAsync(int profissionalId)
        {
            var profissional = await _repository.GetWithEmpresaAsync(profissionalId)
                ?? throw new ArgumentException("Profissional nao encontrado.");

            return new ProfissionalPerfilDTO
            {
                Id = profissional.Id,
                Nome = profissional.Nome,
                Login = profissional.Login,
                Telefone = profissional.Telefone,
                EmpresaId = profissional.EmpresaId,
                EmpresaNome = profissional.Empresa?.RazaoSocial,
                StatusCodigo = profissional.Status,
                Status = profissional.Status.ToString()
            };
        }

        public async Task AtualizarPerfilAsync(int profissionalId, AtualizarProfissionalPerfilDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var profissional = await _repository.GetById(profissionalId)
                ?? throw new ArgumentException("Profissional nao encontrado.");

            if (!string.Equals(profissional.Login, dto.Login, StringComparison.OrdinalIgnoreCase))
            {
                var existente = await _repository.GetByLogin(dto.Login);
                if (existente != null && existente.Id != profissionalId)
                    throw new ArgumentException("Login ja esta sendo utilizado por outro profissional.");
            }

            profissional.Nome = dto.Nome;
            profissional.Telefone = dto.Telefone ?? profissional.Telefone;
            profissional.Login = dto.Login;

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                profissional.Senha = dto.Senha!;
            }

            await _repository.Update(profissional);
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

        private static DateOnly GetWeekStart(DateOnly date)
        {
            var diff = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            return date.AddDays(-diff);
        }

        private static ProfissionalAgendaItemDTO MapToAgendaItem(Agendamento agendamento)
        {
            var dataHora = agendamento.Data.ToDateTime(agendamento.Hora);
            return new ProfissionalAgendaItemDTO
            {
                Id = agendamento.Id,
                DataHora = dataHora,
                ClienteNome = agendamento.Cliente?.Nome ?? $"Cliente #{agendamento.ClienteId}",
                ClienteTelefone = agendamento.Cliente?.Telefone,
                ServicoPrincipal = agendamento.Servicos?.FirstOrDefault()?.ServicoPrestado ?? "Serviço",
                StatusCodigo = agendamento.StatusAgendamento,
                Status = agendamento.StatusAgendamento.ToString(),
                Valor = agendamento.Valor,
                Observacao = agendamento.Observacao,
                PodeConfirmar = agendamento.StatusAgendamento == StatusAgendamento.PENDENTE,
                PodeConcluir = agendamento.StatusAgendamento == StatusAgendamento.CONFIRMADO,
            };
        }

        private static ProfissionalAgendamentoDetalheDTO MapToDetalheDto(Agendamento agendamento)
        {
            var item = MapToAgendaItem(agendamento);
            return new ProfissionalAgendamentoDetalheDTO
            {
                Id = item.Id,
                DataHora = item.DataHora,
                ClienteNome = item.ClienteNome,
                ClienteTelefone = item.ClienteTelefone,
                ServicoPrincipal = item.ServicoPrincipal,
                StatusCodigo = item.StatusCodigo,
                Status = item.Status,
                Valor = item.Valor,
                Observacao = item.Observacao,
                PodeConfirmar = item.PodeConfirmar,
                PodeConcluir = item.PodeConcluir,
                ClienteId = agendamento.ClienteId,
                Servicos = agendamento.Servicos?.Select(s => new ServicoResumoDTO
                {
                    Id = s.Id,
                    Nome = s.ServicoPrestado
                }) ?? Enumerable.Empty<ServicoResumoDTO>()
            };
        }

        private static bool PodeTransicionar(StatusAgendamento atual, StatusAgendamento novo)
        {
            return atual switch
            {
                StatusAgendamento.PENDENTE => novo is StatusAgendamento.CONFIRMADO or StatusAgendamento.CANCELADO or StatusAgendamento.CANCELADO_EMPRESA,
                StatusAgendamento.CONFIRMADO => novo is StatusAgendamento.REALIZADO or StatusAgendamento.CANCELADO or StatusAgendamento.AUSENTE,
                _ => false
            };
        }
    }
}
