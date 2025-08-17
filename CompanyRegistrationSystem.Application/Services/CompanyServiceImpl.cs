using AutoMapper;
using CompanyRegistrationSystem.Application.Dtos;
using CompanyRegistrationSystem.Application.Interface;
using CompanyRegistrationSystem.Domain.Entities;
using CompanyRegistrationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class CompanyServiceImpl : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpService _otpService;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public CompanyServiceImpl(IUnitOfWork unitOfWork, IOtpService otpService, IConfiguration config, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _otpService = otpService;
        _config = config;
        _mapper = mapper;
    }

    public async Task<string> SignUpAsync(CompanySignUpDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ArabicName) || string.IsNullOrWhiteSpace(dto.EnglishName))
            throw new ApplicationException("Company Arabic and English names are required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ApplicationException("Email is required.");

        try { var mail = new System.Net.Mail.MailAddress(dto.Email); }
        catch { throw new ApplicationException("Invalid email format."); }

        var exists = await _unitOfWork.Companies.Query().AnyAsync(c => c.Email.ToLower() == dto.Email.ToLower());
        if (exists) throw new ApplicationException("Email already used.");

        var company = _mapper.Map<Company>(dto);
        company.IsVerified = false;

        await _unitOfWork.Companies.AddAsync(company);
        await _unitOfWork.SaveChangesAsync();

        var otpCode = await _otpService.CreateAndSendOtpAsync(company);

        return otpCode; // لأغراض الاختبار فقط
    }

    public async Task<bool> ValidateOtpAsync(OtpValidationDto dto)
    {
        var company = await _unitOfWork.Companies.Query()
            .FirstOrDefaultAsync(c => c.Email.ToLower() == dto.Email.ToLower());

        if (company == null) return false;

        var ok = await _otpService.ValidateOtpAsync(company, dto.Otp);
        if (ok)
        {
            company.IsVerified = true;
            await _unitOfWork.SaveChangesAsync();
        }

        return ok;
    }

    public async Task SetPasswordAsync(SetPasswordDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
            throw new ApplicationException("Passwords do not match.");

        if (dto.NewPassword.Length <= 6)
            throw new ApplicationException("Password must be more than 6 characters.");
        if (!dto.NewPassword.Any(char.IsUpper))
            throw new ApplicationException("Password must contain at least one uppercase letter.");
        if (!dto.NewPassword.Any(char.IsDigit))
            throw new ApplicationException("Password must contain at least one digit.");
        if (!dto.NewPassword.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new ApplicationException("Password must contain at least one special character.");

        var company = await _unitOfWork.Companies.Query()
            .FirstOrDefaultAsync(c => c.Email.ToLower() == dto.Email.ToLower());

        if (company == null)
            throw new ApplicationException("Company not found.");

        var lastOtp = await _unitOfWork.Otps.Query()
            .Where(o => o.CompanyId == company.Id && !o.IsUsed)
            .OrderByDescending(o => o.ExpiryUtc)
            .FirstOrDefaultAsync();

        if (lastOtp == null)
            throw new ApplicationException("OTP is invalid or expired.");

        var otpExpiryUtc = DateTime.SpecifyKind(lastOtp.ExpiryUtc, DateTimeKind.Utc);

        if (otpExpiryUtc < DateTime.UtcNow || lastOtp.Code != dto.Otp)
            throw new ApplicationException("OTP is invalid or expired.");

        company.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        company.IsVerified = true;

        lastOtp.IsUsed = true;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto dto)
    {
        var company = await _unitOfWork.Companies.Query()
            .FirstOrDefaultAsync(c => c.Email.ToLower() == dto.Email.ToLower());

        if (company == null) throw new ApplicationException("Invalid credentials.");

        if (string.IsNullOrEmpty(company.PasswordHash) || !BCrypt.Net.BCrypt.Verify(dto.Password, company.PasswordHash))
            throw new ApplicationException("Invalid credentials.");

        var jwtSection = _config.GetSection("Jwt");
        var secret = jwtSection.GetValue<string>("Secret") ?? throw new Exception("JWT secret not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, company.Id.ToString()),
        new Claim("companyName", company.EnglishName),
        new Claim(JwtRegisteredClaimNames.Email, company.Email)
    };

        var token = new JwtSecurityToken(
            issuer: jwtSection.GetValue<string>("Issuer"),
            audience: jwtSection.GetValue<string>("Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new LoginResultDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            CompanyName = company.EnglishName,
            Logo = company.LogoPath 
        };
    }

}
