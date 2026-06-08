using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Server.Models;
namespace Server.Services.Interfaces
{
   public interface IAssignmentService
{
    Task<List<Assignment>> RunAlgorithmAsync(int year);
    Task<List<Assignment>> GetByYearAsync(int year);
}
}