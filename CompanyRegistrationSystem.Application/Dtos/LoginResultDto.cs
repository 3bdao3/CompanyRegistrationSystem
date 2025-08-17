using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Dtos
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public string CompanyName { get; set; }
        public string Logo { get; set; }
    }

}
