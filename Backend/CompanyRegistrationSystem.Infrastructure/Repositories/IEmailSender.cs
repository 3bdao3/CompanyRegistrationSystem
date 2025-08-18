using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Infrastructure.Repositories
{
    // Infrastructure/Email/IEmailSender.cs
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    
}
