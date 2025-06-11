using AkariBeauty.Objects.Models;

namespace AkariBeauty.Data.Interfaces
{
    public interface IClienteRepository : IGenericoRepository<Cliente>
    {
        Task<Cliente> GetByLogin(string login);
    }
}
