using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity, TModel> 
        where TEntity : BaseEntity
        where TModel : BaseModel
    {
        Task<ICollection<TModel>> GetWithPaginationAsync(PaginationParameters parameters);
        Task<TModel> CreateAsync(TModel model);
        Task<TModel> UpdateAsync(int id, TModel model);
        Task<int> DeleteAsync(int id);
        Task<TModel> GetAsync(int id);
    }
}
