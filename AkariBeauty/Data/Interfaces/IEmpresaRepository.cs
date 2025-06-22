using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IEmpresaRepository : IGenericoRepository<Empresa>
    {
        Task<Empresa> FindByCnpj(string cnpj);
    }
}
