using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Dtos
{
    // Application/DTOs/CompanySignUpDto.cs
    public class CompanySignUpDto
    {
        public string ArabicName { get; set; } = null!;
        public string EnglishName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }
    }
    






}
