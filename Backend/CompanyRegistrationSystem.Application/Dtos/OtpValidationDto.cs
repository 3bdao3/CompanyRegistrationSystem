using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Dtos
{
    public class OtpValidationDto
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
