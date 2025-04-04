namespace AkariBeauty.Data.Interfaces
{
    public interface IGenericoRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task<bool> SaveChanges();
    }
}
