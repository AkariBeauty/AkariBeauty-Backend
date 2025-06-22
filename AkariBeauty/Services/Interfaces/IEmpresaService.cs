using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Services.Interfaces
{
    public interface IEmpresaService : IGenericoService<Empresa, EmpresaDTO>, IGenericLogin
    {
        new Task<EmpresaDTO> Create(Empresa empresa);
    }
}
