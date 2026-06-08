using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Server.Models; // ודא שה-namespace תואם לקבצים שלך
namespace Server.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string PasswordHash { get; set; } = "";

        public string Role { get; set; }  // Admin / Teacher / etc

        public ICollection<History> Histories { get; set; } = new List<History>();
    }
    public class RegisterRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "Password";
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "System";  }
    }
