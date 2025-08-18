using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Domain.Entities
{
    public class OtpCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CompanyId { get; set; }
        public string Code { get; set; } = null!;
        public DateTime ExpiryUtc { get; set; }
        public bool IsUsed { get; set; } = false;
        public Company? Company { get; set; }
    }


}
