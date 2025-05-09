using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class FuncionarioService : GenericoService<Funcionario>, IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IMapper _mapper;

        public FuncionarioService(IFuncionarioRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _funcionarioRepository = repository;
            _mapper = mapper;
        }
    }
}
