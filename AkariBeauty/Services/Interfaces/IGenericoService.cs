namespace AkariBeauty.Services.Interfaces
{
    public interface IGenericoService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Create(T entity);
        Task Update(T entity, int id);
        Task Remove(int id);
    }
}
