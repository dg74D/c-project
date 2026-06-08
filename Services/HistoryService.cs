using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Models.DTOs;
using Server.Services.Interfaces;
namespace Server.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly AppDbContext _context;

        public HistoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Guid userId, string actionType, string details)
        {
            var userExists = await _context.Users.AnyAsync(x => x.Id == userId);

            if (!userExists)
                return;

            var history = new History
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ActionType = actionType,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.Histories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HistoryDto>> GetAllAsync()
        {
            var count = await _context.Histories.CountAsync();
            return await _context.Histories
                .Select(h => new HistoryDto
                {
                    Id = h.Id,
                    UserId = h.UserId,
                    ActionType = h.ActionType,
                    Details = h.Details,
                    Timestamp = h.Timestamp
                })
                .ToListAsync();
        }
        public async Task<HistoryDto?> GetByIdAsync(Guid id)
        {
            return await _context.Histories
                .Where(h => h.Id == id)
                .Select(h => new HistoryDto
                {
                    Id = h.Id,
                    UserId = h.UserId,
                    ActionType = h.ActionType,
                    Details = h.Details,
                    Timestamp = h.Timestamp
                })
                .FirstOrDefaultAsync();
        }
    }
}