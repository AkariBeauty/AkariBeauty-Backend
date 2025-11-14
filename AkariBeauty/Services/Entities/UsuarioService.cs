using System;
using System.Threading.Tasks;
using AutoMapper;
using AkariBeauty.Authentication;           
using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Enums;           
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;

namespace AkariBeauty.Services.Entities
{
    public class UsuarioService : GenericoService<Usuario, UsuarioDTO>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public UsuarioService(
            IUsuarioRepository repository,
            JwtService jwtService,               
            IMapper mapper
        ) : base(repository, mapper)
        {
            _usuarioRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request?.Login) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Usuário ou senha inválidos.");

            var user = await _usuarioRepository.GetByLogin(request.Login);
            if (user == null)
                throw new ArgumentException("Usuário ou senha inválidos.");

            if (user.Senha != request.Password) 
                throw new ArgumentException("Usuário ou senha inválidos.");

            if (user.TipoUsuario == TipoUsuario.RECEPCIONISTA)
                throw new ArgumentException("Usuário ou senha inválidos.");

            var payload = new TokenPayload(
                UserId: user.Id.ToString(),
                Role: MapTipoUsuarioToRole(user.TipoUsuario), 
                Name: user.Nome,
                Email: user.Login,                             
                EmpresaId: (user.EmpresaId > 0) ? user.Empresa.Id.ToString() : null
                );

            return _jwtService.CreateAccessToken(payload);
        }

        private static string MapTipoUsuarioToRole(TipoUsuario tipo) => tipo switch
        {
            TipoUsuario.ADMIN => "ADMIN",
            TipoUsuario.PROFISSIONAL => "PROFISSIONAL",
            TipoUsuario.RECEPCIONISTA => "RECEPCIONISTA",
            _ => "USUARIO"
        };
    }
}
