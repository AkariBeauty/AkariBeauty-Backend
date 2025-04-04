using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class ServicoService : GenericoService<Servico>, IServicoService
    {
        private readonly IServicoRepository _servicoRepository;
        private readonly IMapper _mapper;

        public ServicoService(IServicoRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _servicoRepository = repository;
            _mapper = mapper;
        }
    }
}
