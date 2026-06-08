using Server.Data;
using Server.Models.DTOs;

namespace Server.Services.Interfaces
{
    public interface IHistoryService
    {
        Task AddAsync(Guid userId, string actionType, string details);

        Task<IEnumerable<HistoryDto>> GetAllAsync();
        Task<HistoryDto?> GetByIdAsync(Guid id);
    }
}