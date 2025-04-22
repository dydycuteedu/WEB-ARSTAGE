
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Login.Services
{
    public interface IPasswordService
    {
        void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
    }

    public class PasswordService : IPasswordService
    {
        private const int ITERATION_COUNT = 10000;
        private const int NUM_BYTES_REQUESTED = 256 / 8;
        private const int SALT_SIZE = 128 / 8;

        public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            // Tạo salt ngẫu nhiên
            byte[] salt = new byte[SALT_SIZE];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Tạo hash từ mật khẩu và salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: ITERATION_COUNT,
                numBytesRequested: NUM_BYTES_REQUESTED));

            // Lưu trữ salt dưới dạng Base64
            passwordSalt = Convert.ToBase64String(salt);
            passwordHash = hashed;
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // Chuyển đổi salt từ Base64 thành mảng byte
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Tạo hash từ mật khẩu đầu vào và salt đã lưu
            string hashOfInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: ITERATION_COUNT,
                numBytesRequested: NUM_BYTES_REQUESTED));

            // So sánh hash mới tạo với hash đã lưu
            return hashOfInput == storedHash;
        }
    }
}