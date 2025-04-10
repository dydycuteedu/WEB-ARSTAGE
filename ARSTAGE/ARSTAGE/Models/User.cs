using System;
using System.ComponentModel.DataAnnotations;

namespace ARSTAGE.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}