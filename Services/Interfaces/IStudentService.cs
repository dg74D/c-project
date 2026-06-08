using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.DTOs;
namespace Server.Services.Interfaces
{
  public interface IStudentService
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student> CreateAsync(CreateStudentRequest request);
    Task<Student?> UpdateAsync(Guid id, UpdateStudentRequest request);
    Task<bool> DeleteAsync(Guid id);
}
}

