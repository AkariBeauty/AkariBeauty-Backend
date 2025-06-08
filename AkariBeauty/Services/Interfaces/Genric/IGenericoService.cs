namespace AkariBeauty.Services.Interfaces
{
    public interface IGenericoService<T, TDto> where T : class where TDto : class
    {
        Task<IEnumerable<TDto>> GetAll();
        Task<TDto> GetById(int id);
        Task Create(T entity);
        Task Update(T entity, int id);
        Task Remove(int id);
    }
}
