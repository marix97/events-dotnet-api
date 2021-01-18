using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Helpers;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Interfaces
{
    public interface IBaseService<TEntity, TModel, TRepository>
        where TEntity : BaseEntity
        where TModel : BaseModel
        where TRepository : IBaseRepository<TEntity, TModel>
    {
        Task<ICollection<TModel>> GetWithPaginationAsync(PaginationParameters parameters);
        Task<TModel> GetModelAsync(int id);
        Task<TModel> CreateModelAsync(TModel model);
        Task<TModel> UpdateModelAsync(int id, TModel model);
        Task<int> DeleteModelAsync(int id);
    }
}
