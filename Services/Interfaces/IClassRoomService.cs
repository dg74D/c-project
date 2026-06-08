using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface IClassRoomService
{
    Task<List<ClassRoomDto>> GetAllAsync();
    Task<ClassRoomDto?> GetByIdAsync(Guid id);
    Task<ClassRoomDto> CreateAsync(CreateClassRoomRequest req);
    Task<ClassRoomDto?> UpdateAsync(Guid id, UpdateClassRoomRequest req);
}
}