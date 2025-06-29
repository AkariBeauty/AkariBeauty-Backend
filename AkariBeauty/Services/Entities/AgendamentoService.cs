    using AkariBeauty.Controllers.Dtos;
    using AkariBeauty.Data.Interfaces;
    using AkariBeauty.Jwt;
    using AkariBeauty.Objects.Dtos.Entities;
    using AkariBeauty.Objects.Models;
    using AkariBeauty.Services.Interfaces;
    using AutoMapper;

    namespace AkariBeauty.Services.Entities
    {
        public class AgendamentoService : GenericoService<Agendamento, AgendamentoDTO>, IAgendamentoService
        {
            private readonly IAgendamentoRepository _agendamentoRepository;
            private readonly IUsuarioRepository _usuario;
            private readonly IMapper _mapper;
            private readonly JwtService _jwt;

            public AgendamentoService(IAgendamentoRepository repository, IUsuarioRepository usuario, IMapper mapper, JwtService jwt) : base(repository, mapper)
            {
                _agendamentoRepository = repository;
                _usuario = usuario;
                _mapper = mapper;
                _jwt = jwt;
            }

            public async Task<IEnumerable<AgendamentoDTO>> GetAgendamentosPorData(RequestAgendamentoForDateDTO request)
            {
                var idusuario = _jwt.GetInfoToken().Id;

                var agendamentos = await _agendamentoRepository.GetAgendamentos(idusuario);

                if (agendamentos == null || !agendamentos.Any())
                    return new List<AgendamentoDTO>();

                DateOnly? dataInicio = null;
                DateOnly? dataFim = null;

                if (request.Semana.HasValue && request.Ano.HasValue && request.Mes.HasValue)
                {
                    var (inicio, fim) = ObterInicioEFimDaSemana(request.Ano.Value, request.Mes.Value, request.Semana.Value);
                    dataInicio = inicio;
                    dataFim = fim;
                }

                else if (request.Ano.HasValue && request.Mes.HasValue && request.Dia.HasValue)
                {
                    if (request.Dia.Value > DateTime.DaysInMonth(request.Ano.Value, request.Mes.Value))
                    {
                        throw new ArgumentException($"Data de início inválida: o dia {request.Dia.Value} não existe no mês {request.Mes.Value}.");
                    }
                    dataInicio = new DateOnly(request.Ano.Value, request.Mes.Value, request.Dia.Value);

                    int anoFim = request.AnoEnd ?? request.Ano.Value;
                    int mesFim = request.MesEnd ?? request.Mes.Value;
                    int ultimoDiaDoMes = DateTime.DaysInMonth(anoFim, mesFim);
                    int diaFim = request.DiaEnd ?? ultimoDiaDoMes;

                    if (diaFim > ultimoDiaDoMes)
                    {
                        throw new ArgumentException($"Data de fim inválida: o dia {diaFim} não existe no mês {mesFim}.");
                    }
                    dataFim = new DateOnly(anoFim, mesFim, diaFim);
                }
                
                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    agendamentos = agendamentos.Where(a => a.Data >= dataInicio.Value && a.Data <= dataFim.Value);
                }

                if (request.StatusAgendamento.HasValue && request.StatusAgendamento != 0)
                {
                    agendamentos = agendamentos.Where(a => a.StatusAgendamento == request.StatusAgendamento.Value);
                }

                return _mapper.Map<IEnumerable<AgendamentoDTO>>(agendamentos.ToList());
            }

        public (DateOnly Inicio, DateOnly Fim) ObterInicioEFimDaSemana(int ano, int mes, int numeroDaSemana)
            {
                if (numeroDaSemana < 1 || numeroDaSemana > 5)
                {
                    throw new ArgumentException("O número da semana deve estar entre 1 e 5.");
                }

                int diaDeReferencia = (numeroDaSemana - 1) * 7 + 1;

                int diasNoMes = DateTime.DaysInMonth(ano, mes);
                if (diaDeReferencia > diasNoMes)
                {
                    diaDeReferencia = diasNoMes;
                }

                var dataDeReferencia = new DateOnly(ano, mes, diaDeReferencia);

                int diaDaSemanaComoInt = (int)dataDeReferencia.DayOfWeek;

                DateOnly inicioDaSemana = dataDeReferencia.AddDays(-diaDaSemanaComoInt);

                DateOnly fimDaSemana = inicioDaSemana.AddDays(6);

                return (Inicio: inicioDaSemana, Fim: fimDaSemana);
            }

        }
    }
