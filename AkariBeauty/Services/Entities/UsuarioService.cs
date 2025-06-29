using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Jwt;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Services.Interfaces;
using AkariBeauty.Utils;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class UsuarioService : GenericoService<Usuario, UsuarioDTO>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        public UsuarioService(IUsuarioRepository repository, JwtService jwt, IMapper mapper) : base(repository, mapper)
        {
            _usuarioRepository = repository;
            _jwtService = jwt;
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

            InfoToken infoToken = new InfoToken();
            infoToken.Id = user.Id;
            infoToken.Tipo = TipoUsuarioSistema.USUARIO;

            return _jwtService.GenerateJwtToken(infoToken);
        }
    }
}
