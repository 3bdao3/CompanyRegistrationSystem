using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Dtos
{
    public class UploadLogoDto
    {
        public IFormFile File { get; set; }
    }

}
