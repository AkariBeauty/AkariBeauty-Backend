// [CHANGED] Pass IDs as string to JwtService and kept business validations + constant-time compare.
using System;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using AkariBeauty.Authentication;
using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AkariBeauty.Objects.Enum;
using JwtService = AkariBeauty.Authentication.JwtService;

namespace AkariBeauty.Services.Entities
{
    public class ClienteService : GenericoService<Cliente, ClienteDTO>, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwt;

        public ClienteService(
             IClienteRepository repository,
             IMapper mapper,
            JwtService jwt
        ) : base(repository, mapper)
        {
            _clienteRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));
        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Normalização/validação mínima (tamanhos e mensagens neutras)
            var login = (request.Login ?? string.Empty).Trim();
            var password = request.Password ?? string.Empty;
            if (login.Length < 3 || login.Length > 150) throw new ArgumentException("Usuário ou senha inválidos.");
            if (password.Length < 6 || password.Length > 128) throw new ArgumentException("Usuário ou senha inválidos.");

            // Busca cliente
            var cliente = await _clienteRepository.GetByLogin(login);
            if (cliente is null) throw new ArgumentException("Usuário ou senha inválidos.");

            // Verificação de senha (substitua por hash quando tiver)
            if (!VerifyPassword(cliente, password)) throw new ArgumentException("Usuário ou senha inválidos.");

            // Emite token com claim clienteId (IDs como string)
            var token = _jwt.CreateAccessToken(new TokenPayload(
             UserId: cliente.Id.ToString(),
             Role: Roles.Cliente.ToString(),  
             ClienteId: cliente.Id.ToString()
));

            return token;
        }

        private static bool VerifyPassword(Cliente cliente, string plainPassword)
        {
            // Placeholder: troque por verificação de hash (BCrypt/Argon2)
            var stored = cliente.Senha ?? string.Empty;
            var a = Encoding.UTF8.GetBytes(stored);
            var b = Encoding.UTF8.GetBytes(plainPassword);
            if (a.Length != b.Length) return false;
            return CryptographicOperations.FixedTimeEquals(a, b);
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
