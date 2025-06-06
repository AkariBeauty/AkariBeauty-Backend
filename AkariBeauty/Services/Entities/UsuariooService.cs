using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class UsuarioService : GenericoService<Usuario, UsuarioDTO>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _usuarioRepository = repository;
            _mapper = mapper;
        }
    }
}
