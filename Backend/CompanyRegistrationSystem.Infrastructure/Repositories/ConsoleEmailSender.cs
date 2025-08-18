using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Infrastructure.Repositories
{
    public class ConsoleEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public ConsoleEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = int.Parse(_config["Smtp:Port"]);
            var smtpUser = _config["Smtp:UserName"];
            var smtpPass = _config["Smtp:Password"];
            var enableSsl = bool.Parse(_config["Smtp:EnableSsl"]);

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                client.EnableSsl = enableSsl;

                var mail = new MailMessage(smtpUser, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mail);
            }
        }
    }

}
