using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Agendamentos;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class AgendamentoService : GenericoService<Agendamento, AgendamentoDTO>, IAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IProfissionalServicoRepository _profissionalServicoRepository;
        private readonly IMapper _mapper;

        private const int BufferMinutes = 15;
        private const int LeadTimeHours = 2;
        private const int MaxWindowDays = 60;
        private const int DefaultDurationMinutes = 60;
        private const int MinimumSlotMinutes = 15;
        private static readonly TimeOnly OpeningTime = new(9, 0);
        private static readonly TimeOnly ClosingTime = new(18, 0);

        public AgendamentoService(
            IAgendamentoRepository repository,
            IServicoRepository servicoRepository,
            IProfissionalServicoRepository profissionalServicoRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _agendamentoRepository = repository;
            _servicoRepository = servicoRepository;
            _profissionalServicoRepository = profissionalServicoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AgendamentoDetalheDTO>> GetByClienteId(int clienteId)
        {
            var agendamentos = await _agendamentoRepository.GetByClienteId(clienteId);
            return agendamentos.Select(MapToDetalheDto);
        }

        public async Task<AgendamentoDetalheDTO> CreateAgendamentoAsync(CriarAgendamentoRequest request)
        {
            var servico = await _servicoRepository.GetById(request.ServicoId)
                ?? throw new ArgumentException("Serviço informado não foi localizado.");

            if (request.ProfissionalId <= 0)
            {
                throw new ArgumentException("O profissional é obrigatório para concluir o agendamento.");
            }

            var relacao = await _profissionalServicoRepository.GetProfissionalAndServico(request.ProfissionalId, request.ServicoId)
                ?? throw new ArgumentException("O profissional selecionado não executa este serviço.");

            if (request.DataHora.Kind == DateTimeKind.Unspecified)
            {
                // Assume horário vindo do front no fuso local do servidor
                request.DataHora = DateTime.SpecifyKind(request.DataHora, DateTimeKind.Local);
            }

            var instanteLocal = request.DataHora.Kind switch
            {
                DateTimeKind.Utc => request.DataHora.ToLocalTime(),
                DateTimeKind.Local => request.DataHora,
                _ => DateTime.SpecifyKind(request.DataHora, DateTimeKind.Local),
            };

            if (instanteLocal <= DateTime.Now)
            {
                throw new ArgumentException("Agendamentos só podem ser criados para datas e horários futuros.");
            }

            var data = DateOnly.FromDateTime(instanteLocal);
            var hora = TimeOnly.FromDateTime(instanteLocal);
            var durationMinutes = await ResolverDuracaoMinutosAsync(request.ServicoId, request.ProfissionalId);
            await ValidarConflitoAsync(data, hora, durationMinutes, request.ProfissionalId);

            var entidade = new Agendamento
            {
                ClienteId = request.ClienteId,
                Data = data,
                Hora = hora,
                Valor = servico.ValorBase,
                Comissao = relacao.Comissao,
                ProfissionalId = request.ProfissionalId,
                Observacao = request.Observacao,
                StatusAgendamento = StatusAgendamento.PENDENTE,
            };

            entidade.Servicos.Add(servico);
            entidade.Profissional = relacao.Profissional;

            await _agendamentoRepository.Add(entidade);

            return MapToDetalheDto(entidade);
        }

        public override async Task Remove(int id)
        {
            var entity = await _agendamentoRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Agendamento com id {id} não encontrado.");

            entity.StatusAgendamento = StatusAgendamento.CANCELADO;

            await _agendamentoRepository.Update(entity);
        }

        public async Task<IEnumerable<DisponibilidadeDiaDTO>> ListarDisponibilidadeAsync(DisponibilidadeFiltro filtro)
        {
            if (filtro.ServicoId <= 0)
            {
                throw new ArgumentException("O identificador do serviço é obrigatório.");
            }

            _ = await _servicoRepository.GetById(filtro.ServicoId)
                ?? throw new ArgumentException("Serviço informado não foi localizado.");

            var now = DateTime.Now;
            var minDate = DateOnly.FromDateTime(now);
            var minStart = now.AddHours(LeadTimeHours);
            var limitDate = DateOnly.FromDateTime(now.AddDays(MaxWindowDays));

            var startDate = filtro.Inicio.HasValue && filtro.Inicio.Value > minDate
                ? filtro.Inicio.Value
                : minDate;

            var endDate = filtro.Fim ?? startDate.AddDays(13);
            if (endDate > limitDate)
            {
                endDate = limitDate;
            }

            if (endDate < startDate)
            {
                throw new ArgumentException("O período informado é inválido.");
            }

            var durationCache = new Dictionary<(int, int?), int>();
            var durationMinutes = await ResolverDuracaoMinutosAsync(filtro.ServicoId, filtro.ProfissionalId, durationCache);
            var existing = await _agendamentoRepository.GetByPeriodo(startDate, endDate, filtro.ServicoId, filtro.ProfissionalId);
            var existingBlocks = new List<(DateTime Inicio, DateTime Fim)>();

            foreach (var agendamento in existing)
            {
                var servicoRelacionadoId = agendamento.Servicos.FirstOrDefault()?.Id ?? filtro.ServicoId;
                if (servicoRelacionadoId == 0)
                {
                    continue;
                }

                var minutos = await ResolverDuracaoMinutosAsync(servicoRelacionadoId, agendamento.ProfissionalId, durationCache);
                var inicio = agendamento.Data.ToDateTime(agendamento.Hora);
                var fim = inicio.AddMinutes(minutos + BufferMinutes);
                existingBlocks.Add((inicio, fim));
            }

            var resultado = new List<DisponibilidadeDiaDTO>();
            var limitDateTime = now.AddDays(MaxWindowDays);

            for (var cursor = startDate; cursor <= endDate; cursor = cursor.AddDays(1))
            {
                if (cursor.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                var slots = new List<string>();
                var openingMinutes = ToMinutes(OpeningTime);
                var closingMinutes = ToMinutes(ClosingTime);
                var lastPossibleStart = closingMinutes - durationMinutes;
                var slotStep = Math.Max(durationMinutes, MinimumSlotMinutes);

                for (var minutes = openingMinutes; minutes <= lastPossibleStart; minutes += slotStep)
                {
                    var time = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(minutes));
                    var candidateStart = cursor.ToDateTime(time);
                    var candidateEnd = candidateStart.AddMinutes(durationMinutes + BufferMinutes);

                    if (candidateStart < minStart)
                    {
                        continue;
                    }

                    if (candidateStart > limitDateTime)
                    {
                        break;
                    }

                    var conflita = existingBlocks.Any(slot =>
                        slot.Inicio < candidateEnd && candidateStart < slot.Fim);

                    if (!conflita)
                    {
                        slots.Add(candidateStart.ToString("HH:mm"));
                    }
                }

                if (slots.Count > 0)
                {
                    resultado.Add(new DisponibilidadeDiaDTO
                    {
                        Data = cursor,
                        Horarios = slots,
                    });
                }
            }

            return resultado;
        }

        private async Task<int> ResolverDuracaoMinutosAsync(int servicoId, int? profissionalId, IDictionary<(int, int?), int>? cache = null)
        {
            var cacheKey = (servicoId, profissionalId);

            if (cache != null && cache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            int minutos;

            if (profissionalId.HasValue)
            {
                var relacao = await _profissionalServicoRepository.GetProfissionalAndServico(profissionalId.Value, servicoId);
                if (relacao != null)
                {
                    minutos = ToMinutes(relacao.Tempo);
                    if (minutos > 0)
                    {
                        if (cache != null)
                        {
                            cache[cacheKey] = minutos;
                        }
                        return minutos;
                    }
                }
            }

            var opcoes = await _profissionalServicoRepository.GetProfissionalServicoForServico(servicoId);
            var melhor = opcoes
                ?.Select(relacao => ToMinutes(relacao.Tempo))
                .Where(m => m > 0)
                .DefaultIfEmpty(DefaultDurationMinutes)
                .Min();

            minutos = melhor ?? DefaultDurationMinutes;
            if (cache != null)
            {
                cache[cacheKey] = minutos;
            }
            return minutos;
        }

        private static int ToMinutes(TimeOnly time) => (time.Hour * 60) + time.Minute;

        private async Task ValidarConflitoAsync(DateOnly data, TimeOnly hora, int duracaoMinutos, int? profissionalId)
        {
            if (!profissionalId.HasValue)
            {
                return;
            }

            var existentes = await _agendamentoRepository.GetByPeriodo(data, data, null, profissionalId);
            if (existentes.Count == 0)
            {
                return;
            }

            var durationCache = new Dictionary<(int, int?), int>();
            var candidatoInicio = data.ToDateTime(hora);
            var candidatoFim = candidatoInicio.AddMinutes(duracaoMinutos + BufferMinutes);

            foreach (var agendamento in existentes)
            {
                var servicoId = agendamento.Servicos.FirstOrDefault()?.Id ?? 0;
                if (servicoId == 0)
                {
                    continue;
                }

                var existenteDuracao = await ResolverDuracaoMinutosAsync(servicoId, profissionalId, durationCache);
                var existenteInicio = agendamento.Data.ToDateTime(agendamento.Hora);
                var existenteFim = existenteInicio.AddMinutes(existenteDuracao + BufferMinutes);

                if (existenteInicio < candidatoFim && candidatoInicio < existenteFim)
                {
                    throw new ArgumentException("O horário escolhido já está reservado para o profissional selecionado.");
                }
            }
        }

        private static AgendamentoDetalheDTO MapToDetalheDto(Agendamento entity)
        {
            var dataHora = entity.Data.ToDateTime(entity.Hora);

            return new AgendamentoDetalheDTO
            {
                Id = entity.Id,
                ClienteId = entity.ClienteId,
                DataHora = dataHora,
                Status = entity.StatusAgendamento.ToString(),
                Valor = entity.Valor,
                Comissao = entity.Comissao,
                Servicos = entity.Servicos?.Select(s => new ServicoResumoDTO
                {
                    Id = s.Id,
                    Nome = s.ServicoPrestado,
                }) ?? Enumerable.Empty<ServicoResumoDTO>(),
                Profissional = entity.Profissional != null
                    ? new ProfissionalResumoDTO
                    {
                        Id = entity.Profissional.Id,
                        Nome = entity.Profissional.Nome,
                        Telefone = entity.Profissional.Telefone,
                    }
                    : null,
                Observacao = entity.Observacao,
            };
        }
    }
}
