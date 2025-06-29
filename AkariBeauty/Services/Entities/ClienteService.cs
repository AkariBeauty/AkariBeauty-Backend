using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Jwt;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Services.Interfaces;
using AkariBeauty.Utils;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class ClienteService : GenericoService<Cliente, ClienteDTO>, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        public ClienteService(IClienteRepository repository, JwtService jwt, IMapper mapper) : base(repository, mapper)
        {
            _clienteRepository = repository;
            _jwtService = jwt;
            _mapper = mapper;
        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            // Receber as informaçẽos
            Cliente cliente = await _clienteRepository.GetByLogin(request.Login);

            // Verificar a existencia 
            if (cliente == null)
                throw new ArgumentException("Usuário ou senha inválidos.");

            // Verificar a senha
            if (cliente.Senha != request.Password)
                throw new ArgumentException("Usuário ou senha inválidos.");

            InfoToken infoToken = new InfoToken();
            infoToken.Id = cliente.Id;
            infoToken.Tipo = TipoUsuarioSistema.CLIENTE;

            // Retornar o token
            return _jwtService.GenerateJwtToken(infoToken);
        }
    }
}
