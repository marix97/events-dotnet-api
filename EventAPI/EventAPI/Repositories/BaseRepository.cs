using AutoMapper;
using EventAPI.CustomExceptions;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Entities.DatabaseContext;
using EventAPI.Helpers;
using EventAPI.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Repositories
{
    public class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel>
        where TEntity : BaseEntity
        where TModel : BaseModel
    {
        protected IMapper _mapper;
        protected EventsAPIContext _context;

        public BaseRepository(IMapper mapper, EventsAPIContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<TModel> CreateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return _mapper.Map<TModel>(entity);
            }
            catch (DbUpdateException e)
            {
                SqlException innerException = e.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                {
                    throw new UniqueEmailException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var modelToDelete = await _context.Set<TEntity>().FindAsync(id);

            if (!(modelToDelete is null))
            {
                _context.Set<TEntity>().Remove(modelToDelete);
                await _context.SaveChangesAsync();
                return 1;
            }

            return default;
        }


        public async Task<TModel> GetAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            return _mapper.Map<TModel>(entity);
        }

        public async Task<ICollection<TModel>> GetWithPaginationAsync(PaginationParameters parameters)
        {
            var entities = await _context.Set<TEntity>()
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<ICollection<TModel>>(entities);
        }

        public async Task<TModel> UpdateAsync(int id, TModel model)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            try
            {
                _context.Entry(entity).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return _mapper.Map<TModel>(entity);
            }
            catch (DbUpdateException e)
            {
                SqlException innerException = e.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                {
                    throw new UniqueEmailException();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
