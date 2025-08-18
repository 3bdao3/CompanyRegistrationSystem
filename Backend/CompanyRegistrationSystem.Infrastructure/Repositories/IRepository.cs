using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq.Expressions;


namespace CompanyRegistrationSystem.Infrastructure.Repositories
{

    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        Task SaveChangesAsync();
    }

}
