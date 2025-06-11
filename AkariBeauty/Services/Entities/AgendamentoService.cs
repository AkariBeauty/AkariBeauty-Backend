using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class AgendamentoService : GenericoService<Agendamento, AgendamentoDTO>, IAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IMapper _mapper;

        public AgendamentoService(IAgendamentoRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _agendamentoRepository = repository;
            _mapper = mapper;
        }
    }
}
