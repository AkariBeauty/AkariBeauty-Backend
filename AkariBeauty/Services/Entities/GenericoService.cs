﻿using AkariBeauty.Data.Interfaces;
using AkariBeauty.Services.Interfaces;
using AutoMapper;

namespace AkariBeauty.Services.Entities
{
    public class GenericoService<T> : IGenericoService<T> where T : class
    {
        private readonly IGenericoRepository<T> _repository;
        private readonly IMapper _mapper;
        private IMapper mapper;

        public GenericoService(IGenericoRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<T>> GetAll()
        {
            var entities = await _repository.Get();
            return _mapper.Map<IEnumerable<T>>(entities);
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<T>(entity);
        }

        public async Task Create(T entity)
        {
            await _repository.Add(entity);
        }

        public async Task Update(T entity, int id)
        {
            var existingEntity = await _repository.GetById(id); // Supondo que sua entidade tenha um campo Id

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            await _repository.Update(entity);
        }

        public async Task Remove(int id)
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

/*namespace AkariBeauty.Services.Entities
{
    public class GenericoService : IGenericoService<T> where T : class
    {
        private readonly IGenericoRepository<T> _repository;
        private readonly IMapper _mapper;
        private IServicoRepository repository;
        private IMapper mapper;

        public GenericoService(IGenericoRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

       public GenericoService(IServicoRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var entities = await _repository.Get();
            return _mapper.Map<IEnumerable<T>>(entities);
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<T>(entity);
        }

        public async Task Create(T entity)
        {
            await _repository.Add(entity);
        }

        public async Task Update(T entity, int id)
        {
            var existingEntity = await _repository.GetById(id); // Supondo que sua entidade tenha um campo Id

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            await _repository.Update(entity);
        }

        public async Task Remove(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entidade com id: {id} não encontrado");
            }

            await _repository.Remove(entity);
        }
    }
}*/
