using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ARSTAGE.Models;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ARSTAGE.Data;

namespace ARSTAGE.Data
{
    public interface IUserRepository
    {
        Task<AppUserModel> GetUserByUsernameAsync(string username);
        Task<AppUserModel> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(AppUserModel user);
        Task UpdateLastLoginDateAsync(string id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUserModel> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<AppUserModel> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }


        public async Task<bool> CreateUserAsync(AppUserModel user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateLastLoginDateAsync(string Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

    }
    public interface IUserPasswordResetRepository
    {
        Task<AppUserModel> GetUserByEmailAsync(string email);
        Task UpdateResetPasswordTokenAsync(string email, string token, DateTime expiry);
        Task ResetPasswordAsync(string email, string token, string newPasswordHash);

    }


    public class UserPasswordResetRepository : IUserPasswordResetRepository
    {
        private readonly string _connectionString;

        public UserPasswordResetRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<AppUserModel> GetUserByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT UserId, Email, PasswordHash, ResetPasswordToken, ResetPasswordTokenExpiry FROM Users WHERE Email = @Email", connection);
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AppUserModel
                {
                    Id = reader.GetString(0),
                    Email = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    ResetPasswordToken = reader.IsDBNull(3) ? null : reader.GetString(3),
                    ResetPasswordTokenExpiry = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                };
            }

            return null;
        }

        public async Task UpdateResetPasswordTokenAsync(string email, string token, DateTime expiry)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("UPDATE Users SET ResetPasswordToken = @Token, ResetPasswordTokenExpiry = @Expiry WHERE Email = @Email", connection);
            command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;
            command.Parameters.Add("@Expiry", SqlDbType.DateTime).Value = expiry;
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

            await command.ExecuteNonQueryAsync();
        }

        public async Task ResetPasswordAsync(string email, string token, string newPasswordHash)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "UPDATE Users SET PasswordHash = @PasswordHash, ResetPasswordToken = NULL, ResetPasswordTokenExpiry = NULL " +
                "WHERE Email = @Email AND ResetPasswordToken = @Token AND ResetPasswordTokenExpiry > @Now", connection);
            command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar).Value = newPasswordHash;
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
            command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;
            command.Parameters.Add("@Now", SqlDbType.DateTime).Value = DateTime.UtcNow;

            await command.ExecuteNonQueryAsync();
        }
    }
}