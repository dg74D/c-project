using Server.Models;
using Microsoft.EntityFrameworkCore;
namespace Server.Services.Interfaces
{
  
    
        public interface IUserService
        {
            Task<UserDto> RegisterAsync(RegisterRequest request);
            Task<string?> LoginAsync(LoginRequest request); // JWT
            Task<UserDto?> GetMeAsync(Guid userId);

        }
    
}