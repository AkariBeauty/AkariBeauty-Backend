using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Data.Interfaces;
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

        public async Task<string> Login(RequestLogin request)
        {
            return "";
        }
    }
}
