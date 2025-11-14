using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Jwt;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class ClienteService : GenericoService<Cliente, ClienteDTO>, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        public ClienteService(IClienteRepository repository, IConfiguration configuration, IMapper mapper) : base(repository, mapper)
        {
            _clienteRepository = repository;
            _jwtService = new JwtService(configuration);
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

            // Retornar o token
            return _jwtService.GenerateJwtToken(TipoUsuarioSistema.CLIENTE.ToString(), cliente.Id.ToString());
        }

        public async Task ChangePasswordAsync(int clienteId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Nova senha inválida.");

            var cliente = await _clienteRepository.GetById(clienteId);

            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado.");

            if (!string.Equals(cliente.Senha, currentPassword))
                throw new ArgumentException("Senha atual incorreta.");

            cliente.Senha = newPassword;
            await _clienteRepository.Update(cliente);
        }
    }
}
