using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Data.Repositories;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class EmpresaService : GenericoService<Empresa>, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;

        public EmpresaService(IEmpresaRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _empresaRepository = repository;
            _mapper = mapper;
        }

        public async Task Create(EmpresaComUsuarioDTO dto)
        {

            Empresa entity = dto.Empresa;
            Usuario usuario = dto.Usuario;
            
            var empresa = await _empresaRepository.FindByCnpj(entity.Cnpj);
            if (empresa != null)
                throw new Exception("Empresa ja cadastrada");

            await _empresaRepository.Add(entity);

            empresa = await _empresaRepository.FindByCnpj(entity.Cnpj);

            usuario.Empresa = empresa;
            usuario.TipoUsuario = Objects.Enums.TipoUsuario.ADMIN;

            empresa.Usuarios?.Add(usuario);

            await _empresaRepository.SaveChanges();

        }


        public async Task<string> Login(RequestLogin request)
        {
            return "";
        }
    }
}
