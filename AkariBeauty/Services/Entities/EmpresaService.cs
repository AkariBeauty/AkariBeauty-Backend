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
    public class EmpresaService : GenericoService<Empresa, EmpresaDTO>, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public EmpresaService(
            IEmpresaRepository repository,
            IUsuarioRepository usuarioRepository,
            JwtService jwtService,
            IMapper mapper
        ) : base(repository, mapper)
        {
            _empresaRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<EmpresaDTO> Create(Empresa entity)
        {
            var existente = await _empresaRepository.FindByCnpj(entity.Cnpj);
            if (existente != null)
                throw new Exception("Empresa ja cadastrada");

            var usuario = entity.Usuarios?.FirstOrDefault();
            if (usuario == null)
                throw new Exception("Informe o usuário!");

            entity.Usuarios = null;
            await _empresaRepository.Add(entity);

            var empresa = await _empresaRepository.FindByCnpj(entity.Cnpj);
            if (empresa == null) throw new Exception("Falha ao criar a empresa.");

            empresa.Usuarios ??= new System.Collections.Generic.List<Usuario>();

            usuario.Empresa = empresa;
            usuario.TipoUsuario = TipoUsuario.ADMIN;
            empresa.Usuarios.Add(usuario);

            await _empresaRepository.SaveChanges();

            return _mapper.Map<EmpresaDTO>(empresa);
        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request?.Login) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Usuário ou senha inválidos.");

            var usuario = await _usuarioRepository.GetByLogin(request.Login);
            if (usuario is null) throw new ArgumentException("Usuário ou senha inválidos.");

            if (usuario.TipoUsuario != TipoUsuario.ADMIN) 
                throw new ArgumentException("Usuário ou senha inválidos.");

            if (usuario.Senha != request.Password)       
                throw new ArgumentException("Usuário ou senha inválidos.");

            var payload = new TokenPayload(
                UserId: usuario.Id.ToString(),
                Role: MapTipoUsuarioToRole(usuario.TipoUsuario), 
                Name: usuario.Nome,
                EmpresaId: (usuario.EmpresaId > 0) ? usuario.EmpresaId.ToString() : null

            );

            return _jwtService.CreateAccessToken(payload);
        }

        private static string MapTipoUsuarioToRole(TipoUsuario tipo) => tipo switch
        {
            TipoUsuario.ADMIN => "ADMIN",
            TipoUsuario.PROFISSIONAL => "PROFISSIONAL",
            TipoUsuario.RECEPCIONISTA => "RECEPCIONISTA",
            _ => "ADMIN"
        };
    }
}
