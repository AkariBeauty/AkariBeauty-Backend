using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Agendamentos;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkariBeauty.Services.Entities
{
    public class AgendamentoService : GenericoService<Agendamento, AgendamentoDTO>, IAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IMapper _mapper;

        public AgendamentoService(IAgendamentoRepository repository, IServicoRepository servicoRepository, IMapper mapper) : base(repository, mapper)
        {
            _agendamentoRepository = repository;
            _servicoRepository = servicoRepository;
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

            var entidade = new Agendamento
            {
                ClienteId = request.ClienteId,
                Data = DateOnly.FromDateTime(instanteLocal),
                Hora = TimeOnly.FromDateTime(instanteLocal),
                Valor = servico.ValorBase,
                Comissao = 0,
                StatusAgendamento = StatusAgendamento.PENDENTE,
            };

            entidade.Servicos.Add(servico);

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
                }) ?? Enumerable.Empty<ServicoResumoDTO>()
            };
        }
    }
}
