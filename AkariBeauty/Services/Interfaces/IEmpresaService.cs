using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;

namespace AkariBeauty.Services.Interfaces
{
    public interface IEmpresaService : IGenericoService<Empresa, EmpresaDTO>, IGenericLogin
    {
        new Task<EmpresaDTO> Create(Empresa empresa);
        Task<UsuarioDTO> GetUser(string token);
        Task<int> GetNovosClientes(DateOnly only);
        Task<float> GetTaxaTotalDeOcupacao(DateOnly only);
    }
}
