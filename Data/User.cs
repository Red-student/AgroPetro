using System.ComponentModel.DataAnnotations;

namespace OpenEcologyApp.Data
{
    public enum UserStatus
    {
        Active,
        Blocked
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    }
} 