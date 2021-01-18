using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Helpers;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain
{
    public abstract class BaseService<TEntity, TModel, TRepository> : IBaseService<TEntity, TModel, TRepository>
        where TEntity : BaseEntity
        where TModel : BaseModel
        where TRepository : IBaseRepository<TEntity, TModel>
    {
        protected readonly TRepository _repository;

        protected BaseService(TRepository repository)
        {
            this._repository = repository;
        }

        public async Task<TModel> CreateModelAsync(TModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<int> DeleteModelAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<TModel> GetModelAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<ICollection<TModel>> GetWithPaginationAsync(PaginationParameters parameters)
        {
            return await _repository.GetWithPaginationAsync(parameters);
        }

        public async Task<TModel> UpdateModelAsync(int id, TModel model)
        {
            return await _repository.UpdateAsync(id, model);
        }
    }
}
