namespace Server.Models.DTOs.Users
{
    public class RegisterRequest
    {
        public string Email { get; set; }   
        public string Password { get; set; } = "";
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
        public string Role { get; set; }
    }
}