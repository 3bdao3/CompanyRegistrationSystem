using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ArabicName { get; set; } = null!;
        public string EnglishName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }



}
