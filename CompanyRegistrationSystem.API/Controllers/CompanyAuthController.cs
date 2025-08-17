using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompanyRegistrationSystem.Application.Dtos;
using CompanyRegistrationSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using CompanyRegistrationSystem.Application.Interface;
using CompanyRegistrationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyRegistrationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(ICompanyService companyService, IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _companyService = companyService;
            _env = env;
            _unitOfWork=unitOfWork;
        }

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo([FromForm] UploadLogoDto dto)
        {
            var file = dto.File;

            if (file == null || file.Length == 0) return BadRequest("No file provided.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".png", ".jpg", ".jpeg", ".gif" };
            if (!allowed.Contains(ext)) return BadRequest("Invalid image format.");

            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "logos");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var filename = $"{Guid.NewGuid()}{ext}";
            var filepath = Path.Combine(uploads, filename);
            using (var stream = System.IO.File.Create(filepath))
            {
                await file.CopyToAsync(stream);
            }

            // هنا نرجع اللينك كامل مش نسبي
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullPath = $"{baseUrl}/logos/{filename}";

            return Ok(new { Path = fullPath });
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] CompanySignUpDto dto)
        {
            try
            {
                var otp = await _companyService.SignUpAsync(dto);
                // Return OTP in response for tooltip/testing (task requirement).
                return Ok(new { Message = "OTP sent to email.", Otp = otp });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] OtpValidationDto dto)
        {
            var ok = await _companyService.ValidateOtpAsync(dto);
            if (!ok) return BadRequest(new { Error = "Invalid or expired OTP." });
            return Ok();
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            try
            {
                await _companyService.SetPasswordAsync(dto);
                return Ok(new { Message = "Password set successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _companyService.LoginAsync(dto);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("home")]
        public async Task<IActionResult> Home()
        {
            var companyIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(companyIdClaim))
                return Unauthorized();

            if (!Guid.TryParse(companyIdClaim, out var companyId))
                return Unauthorized();

            var company = await _unitOfWork.Companies.Query()
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                return NotFound();

            // بناء رابط الصورة
            string logoPath = null;
            if (!string.IsNullOrEmpty(company.LogoPath))
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                logoPath = $"{baseUrl}/logos/{company.LogoPath}";
            }

            return Ok(new
            {
                company.EnglishName,
                LogoPath = logoPath
            });
        }

    }
}
