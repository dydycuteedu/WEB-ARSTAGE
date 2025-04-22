using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ARSTAGE.Data;
using ARSTAGE.Models;
using ARSTAGE.Models.ViewModels;

namespace ARSTAGE.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message, string token)> LoginAsync(LoginViewModel model);
        Task<(bool success, string message)> RegisterAsync(RegisterViewModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private const int ITERATION_COUNT = 10000;
        private const int NUM_BYTES_REQUESTED = 256 / 8;
        private const int SALT_SIZE = 128 / 8;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordService = passwordService;
        }

        public async Task<(bool success, string message, string token)> LoginAsync(LoginViewModel model)
        {
            // Cố gắng lấy người dùng bằng username trước
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            // Nếu không tìm thấy người dùng bằng username, thử tìm bằng email
            if (user == null)
            {
                // Kiểm tra xem đầu vào có giống định dạng email không
                if (model.Username.Contains("@"))
                {
                    user = await _userRepository.GetUserByEmailAsync(model.Username);
                }

                // Nếu vẫn không tìm thấy người dùng, trả về lỗi
                if (user == null)
                {
                    return (false, "Tên đăng nhập hoặc mật khẩu không đúng", null);
                }
            }

            if (!_passwordService.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return (false, "Tên đăng nhập hoặc mật khẩu không đúng", null);
            }

            await _userRepository.UpdateLastLoginDateAsync(user.Id);

            var token = GenerateJwtToken(user);

            return (true, "Đăng nhập thành công", token);
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterViewModel model)
        {
            if (await _userRepository.GetUserByUsernameAsync(model.Username) != null)
            {
                return (false, "Tên đăng nhập đã tồn tại");
            }

            if (await _userRepository.GetUserByEmailAsync(model.Email) != null)
            {
                return (false, "Email đã được sử dụng");
            }

            string passwordHash, passwordSalt;
            _passwordService.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            var user = new AppUserModel
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var result = await _userRepository.CreateUserAsync(user);

            if (result)
            {
                return (true, "Đăng ký thành công");
            }

            return (false, "Đã xảy ra lỗi khi đăng ký");
        }

        private string GenerateJwtToken(AppUserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}