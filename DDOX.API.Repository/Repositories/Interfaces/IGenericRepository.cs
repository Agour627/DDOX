using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Repository.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
        Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression = null,
                                            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }
}
