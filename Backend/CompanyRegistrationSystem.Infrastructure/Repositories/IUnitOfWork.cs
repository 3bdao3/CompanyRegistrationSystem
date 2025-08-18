using CompanyRegistrationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Company> Companies { get; }
        IRepository<OtpCode> Otps { get; }
        Task<int> SaveChangesAsync();
    }

}
