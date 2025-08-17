using CompanyRegistrationSystem.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Interface
{
    public interface ICompanyService
    {
        Task<string> SignUpAsync(CompanySignUpDto dto);
        Task<bool> ValidateOtpAsync(OtpValidationDto dto);

        Task SetPasswordAsync(SetPasswordDto dto);
        Task<LoginResultDto> LoginAsync(LoginDto dto);
    }



}
