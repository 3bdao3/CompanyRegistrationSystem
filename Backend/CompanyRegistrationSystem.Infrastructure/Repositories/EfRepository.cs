using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace CompanyRegistrationSystem.Infrastructure.Repositories
{
    // Infrastructure/Repositories/EfRepository.cs
    using Microsoft.EntityFrameworkCore;
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;
        private readonly DbSet<T> _db;
        public EfRepository(AppDbContext ctx) { _ctx = ctx; _db = ctx.Set<T>(); }
        public async Task AddAsync(T entity) => await _db.AddAsync(entity);
        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) =>
            await _db.FirstOrDefaultAsync(predicate);
        public IQueryable<T> Query() => _db.AsQueryable();
        public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
    }
}



