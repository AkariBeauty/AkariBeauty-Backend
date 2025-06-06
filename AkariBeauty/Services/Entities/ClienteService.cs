using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class ClienteService : GenericoService<Cliente, ClienteDTO>, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _clienteRepository = repository;
            _mapper = mapper;
        }
    }
}
