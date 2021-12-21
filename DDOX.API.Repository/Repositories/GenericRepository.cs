using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Data;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw new Exception($"Couldn't Retrieve Data:{ex.Message}");
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw new Exception($"Couldn't Retrieve Data:{ex.Message}");
            }
        }

        public async Task<T> Add(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw new Exception($"{nameof(entity)} Couldn't be Added:{ex.Message}");
            }
        }
        public async Task<T> Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                T entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                    return false;

                _context.Set<T>().Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw new Exception($" Could not be Deleted: {ex.Message}");
            }
        }

        public async Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression = null,
                                                     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (expression != null)
                    query = query.Where(expression);

                if (include != null)
                    query = include(query);

                if (orderBy != null)
                    return await orderBy(query).ToListAsync();

                return await query.ToListAsync();
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw new Exception($"Couldn't Retrieve Data:{ex.Message}");
            }
        }


    }
}
