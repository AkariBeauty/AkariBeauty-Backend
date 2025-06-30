using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IEmpresaRepository : IGenericoRepository<Empresa>
    {
        Task<Empresa> FindByCnpj(string cnpj);
        Task<IEnumerable<Cliente>> GetNovosClientes(DateOnly only, int idusuario);
        Task<IEnumerable<Profissional>> GetAgendamentosPorProfissionalEData(int idusuario, DateOnly only);
    }
}
