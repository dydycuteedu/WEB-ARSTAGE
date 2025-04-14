using System.Security.Cryptography;
using System.Text;
using ARSTAGE.Data;
using ARSTAGE.Models.ViewModels;
using ARSTAGE.Services;
using Microsoft.AspNetCore.Mvc;

namespace ARSTAGE.Controllers
{
    public class FRPasswordController : Controller
    {
        private readonly IUserPasswordResetRepository _userPasswordResetRepository;
        private readonly IEmailService _emailService;

        public FRPasswordController(IUserPasswordResetRepository userPasswordResetRepository, IEmailService emailService)
        {
            _userPasswordResetRepository = userPasswordResetRepository;
            _emailService = emailService;
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userPasswordResetRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                // Không tiết lộ cho người dùng biết email không tồn tại
                return View("ForgotPasswordConfirmation");
            }

            // Tạo token ngẫu nhiên
            var token = GenerateResetToken();
            var tokenExpiry = DateTime.UtcNow.AddHours(1);

            // Lưu token vào database
            await _userPasswordResetRepository.UpdateResetPasswordTokenAsync(user.Email, token, tokenExpiry);

            // Gửi email
            await _emailService.SendResetPasswordEmailAsync(user.Email, token);

            return View("ForgotPasswordConfirmation");
        }

        // GET: /Account/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Token hoặc email không hợp lệ.");
            }

            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userPasswordResetRepository.GetUserByEmailAsync(model.Email);
            if (user == null || user.ResetPasswordToken != model.Token || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                // Reset token không hợp lệ hoặc đã hết hạn
                ModelState.AddModelError(string.Empty, "Token đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
                return View(model);
            }

            // Mã hóa mật khẩu mới
            string newPasswordHash = HashPassword(model.Password);

            // Cập nhật mật khẩu mới
            await _userPasswordResetRepository.ResetPasswordAsync(model.Email, model.Token, newPasswordHash);

            return View("ResetPasswordConfirmation");
        }

        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // Helper methods
        private string GenerateResetToken()
        {
            using (var rng = RandomNumberGenerator.Create())    
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        private string HashPassword(string password)
        {
            // Đây là một ví dụ đơn giản về hàm băm mật khẩu
            // Trong thực tế, bạn nên sử dụng BCrypt, PBKDF2, hoặc các thư viện băm mật khẩu chuyên dụng
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}