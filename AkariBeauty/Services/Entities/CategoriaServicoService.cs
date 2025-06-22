using AkariBeauty.Data;
using AkariBeauty.Data.Interfaces;
using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities;

public class CategoriaServicoService : ICategoriaServicoService
{
    private readonly ICategoriaServicoRepository _repository;
    private readonly IMapper _mapper;

    public CategoriaServicoService(ICategoriaServicoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaServicoDTO>> GetAll()
    {
        var categorias = await _repository.Get();
        return _mapper.Map<IEnumerable<CategoriaServicoDTO>>(categorias);
    }

    public async Task<CategoriaServicoDTO> GetById(int id)
    {
        CategoriaServico categoria = await _repository.GetById(id);

        if (categoria == null)
            throw new ArgumentException("Categoria nao encontrada");

        return _mapper.Map<CategoriaServicoDTO>(categoria);
    }
}
