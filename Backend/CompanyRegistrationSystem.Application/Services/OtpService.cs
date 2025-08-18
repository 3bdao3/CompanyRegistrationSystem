using CompanyRegistrationSystem.Application.Interface;
using CompanyRegistrationSystem.Domain.Entities;
using CompanyRegistrationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyRegistrationSystem.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly TimeSpan _otpValidity = TimeSpan.FromMinutes(10);

        public OtpService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task<string> CreateAndSendOtpAsync(Company company)
        {
            // إلغاء صلاحية كل الأكواد السابقة للشركة
            var oldOtps = await _unitOfWork.Otps.Query()
                .Where(o => o.CompanyId == company.Id && !o.IsUsed)
                .ToListAsync();

            foreach (var oldOtp in oldOtps)
            {
                oldOtp.IsUsed = true;
            }
            await _unitOfWork.SaveChangesAsync();

            // إنشاء كود جديد
            var rng = new Random();
            var code = rng.Next(100000, 999999).ToString();

            var otp = new OtpCode
            {
                CompanyId = company.Id,
                Code = code,
                ExpiryUtc = DateTime.UtcNow.Add(_otpValidity), // تخزين بتوقيت UTC
                IsUsed = false
            };

            await _unitOfWork.Otps.AddAsync(otp);
            await _unitOfWork.SaveChangesAsync();

            // إرسال الكود بالإيميل
            var body = $"Your OTP code is: {code} (valid for {_otpValidity.TotalMinutes} minutes)";
            await _emailSender.SendEmailAsync(company.Email, "Your OTP Code", body);

            return code;
        }

        public async Task<bool> ValidateOtpAsync(Company company, string otp)
        {
            var allOtps = await _unitOfWork.Otps.Query()
                .Where(o => o.CompanyId == company.Id)
                .OrderByDescending(o => o.ExpiryUtc)
                .ToListAsync();

            Console.WriteLine("[DEBUG] All OTPs for this company:");
            foreach (var o in allOtps)
            {
                var expiryUtc = DateTime.SpecifyKind(o.ExpiryUtc, DateTimeKind.Utc);
                Console.WriteLine($"Code: {o.Code} | IsUsed: {o.IsUsed} | ExpiryUtc(UTC): {expiryUtc} | SecondsRemaining: {(expiryUtc - DateTime.UtcNow).TotalSeconds}");
            }

            var record = allOtps.FirstOrDefault(o => o.Code == otp && !o.IsUsed);
            if (record == null)
            {
                Console.WriteLine("[DEBUG] OTP validation failed: No matching unused record found.");
                return false;
            }

            var recordExpiryUtc = DateTime.SpecifyKind(record.ExpiryUtc, DateTimeKind.Utc);

            if (DateTime.UtcNow > recordExpiryUtc)
            {
                Console.WriteLine("[DEBUG] OTP validation failed: OTP expired.");
                return false;
            }

            // مفيش Mark as Used هنا — هيتعمل في SetPasswordAsync بعد إدخال الباسورد
            Console.WriteLine("[DEBUG] OTP validated successfully — waiting for password step.");
            return true;
        }
    }
}
