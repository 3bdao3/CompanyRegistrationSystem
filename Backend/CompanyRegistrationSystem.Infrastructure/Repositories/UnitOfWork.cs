using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Infrastructure.Repositories
{
    using CompanyRegistrationSystem.Domain.Entities;
    using CompanyRegistrationSystem.Infrastructure.Repositories;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Company> Companies { get; }
        public IRepository<OtpCode> Otps { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Companies = new EfRepository<Company>(_context);
            Otps = new EfRepository<OtpCode>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
