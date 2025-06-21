using AkariBeauty.Data.Interfaces;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class GenericoService<T, TDto> : IGenericoService<T, TDto> where T : class where TDto : class
    {
        private readonly IGenericoRepository<T> _repository;
        private readonly IMapper _mapper;

        public GenericoService(IGenericoRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public virtual async Task<IEnumerable<TDto>> GetAll()
        {
            var entities = await _repository.Get();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual async Task<TDto> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task Create(T entity)
        {
            await _repository.Add(entity);
        }

        public virtual async Task Update(T entity, int id)
        {
            var existingEntity = await _repository.GetById(id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            await _repository.Update(entity);
        }

        public virtual async Task Remove(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entidade com id: {id} não encontrado");
            }

            await _repository.Remove(entity);
        }
    }
}

