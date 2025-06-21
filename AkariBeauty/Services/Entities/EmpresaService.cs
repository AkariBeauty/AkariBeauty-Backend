using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Data.Repositories;
using AkariBeauty.Jwt;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Enums;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Entities.Enum;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class EmpresaService : GenericoService<Empresa, EmpresaDTO>, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;

        private readonly IUsuarioRepository _usuarioRepository;

        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        public EmpresaService(IEmpresaRepository repository, IUsuarioRepository usuarioRepository, IConfiguration configuration, IMapper mapper) : base(repository, mapper)
        {
            _empresaRepository = repository;
            _usuarioRepository = usuarioRepository;
            _jwtService = new JwtService(configuration);
            _mapper = mapper;
        }

        public override async Task<EmpresaDTO> Create(Empresa entity)
        {

            var empresa = await _empresaRepository.FindByCnpj(entity.Cnpj);
            if (empresa != null)
                throw new Exception("Empresa ja cadastrada");

            var usuario = entity.Usuarios?.FirstOrDefault() ?? new Usuario();
            if (usuario == null)
                throw new Exception("Informe o usuário!");

            entity.Usuarios = null;

            await _empresaRepository.Add(entity);

            empresa = await _empresaRepository.FindByCnpj(entity.Cnpj);

            if (empresa.Usuarios == null)
                empresa.Usuarios = new List<Usuario>();

            usuario.Empresa = empresa;
            usuario.TipoUsuario = Objects.Enums.TipoUsuario.ADMIN;
            empresa.Usuarios.Add(usuario);

            await _empresaRepository.SaveChanges();

            return _mapper.Map<EmpresaDTO>(empresa);

        }

        public async Task<string> Login(RequestLoginDTO request)
        {
            // Receber as informaçẽos
            Usuario usuario = await _usuarioRepository.GetByLogin(request.Login);

            // Verificar a existencia 
            if (usuario == null)
                throw new ArgumentException("Usuário ou senha inválidos.");

            // Buscar a os funcioarios que tem o tipo ADMIN
            if (usuario.TipoUsuario != TipoUsuario.ADMIN)
                throw new ArgumentException("Usuário ou senha inválidos.");

            // Verificar a senha
            if (usuario.Senha != request.Password)
                throw new ArgumentException("Usuário ou senha inválidos.");

            // Retornar o token
            return _jwtService.GenerateJwtToken(TipoUsuarioSistema.ADMINISTRADOR.ToString(), usuario.Id.ToString());
        }
    }
}
