using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Jwt;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class UsuarioService : GenericoService<Usuario, UsuarioDTO>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        public UsuarioService(IUsuarioRepository repository, IConfiguration configuration, IMapper mapper) : base(repository, mapper)
        {
            _usuarioRepository = repository;
            _jwtService = new JwtService(configuration);
            _mapper = mapper;
        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            Usuario user = await _usuarioRepository.GetByLogin(request.Login);

            if (user == null)
                throw new ArgumentException("Usuário ou senha inválidos.");

            if (user.Senha != request.Password)
                throw new ArgumentException("Usuário ou senha inválidos.");

            if (user.TipoUsuario == TipoUsuario.ADMIN)
                throw new ArgumentException("Usuário ou senha inválidos.");

            return _jwtService.GenerateJwtToken(TipoUsuarioSistema.USUARIO.ToString(), user.Id.ToString());
        }
    }
}
