using AkariBeauty.Controllers.Dtos;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Services.Interfaces
{
    public interface IEmpresaService : IGenericoService<Empresa>
    {
        Task Create(EmpresaComUsuarioDTO dto);
    }
}
