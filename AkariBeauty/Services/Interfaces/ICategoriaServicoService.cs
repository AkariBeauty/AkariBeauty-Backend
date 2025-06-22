using AkariBeauty.Objects.Dtos.Entities;

namespace AkariBeauty.Services.Interfaces;

public interface ICategoriaServicoService
{
    Task<IEnumerable<CategoriaServicoDTO>> GetAll();
    Task<CategoriaServicoDTO> GetById(int id);
}
