using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Services;
using System.Security.Cryptography;
using System.Text;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _auth;

        public AuthController(AppDbContext context, AuthService auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var exists = await _context.Users.AnyAsync(x => x.Email == req.Email);
            if (exists) return BadRequest("User exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = req.Email,
                PasswordHash = Hash(req.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var hash = Hash(req.Password);

            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Email == req.Email && x.PasswordHash == hash);

            if (user == null)
                return Unauthorized();

            var token = _auth.GenerateToken(user);

            return Ok(new { token });
        }

        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}