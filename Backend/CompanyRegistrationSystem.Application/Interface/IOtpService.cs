using CompanyRegistrationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Interface
{
    public interface IOtpService
    {
        Task<string> CreateAndSendOtpAsync(Company company);
        Task<bool> ValidateOtpAsync(Company company, string otp);
    }

}
