using Server.Data;
using Server.Models;
using Server.Models.DTOs;
using Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class ClassRoomService : IClassRoomService
    {
        private readonly AppDbContext _context;

        public ClassRoomService(AppDbContext context)
        {
            _context = context;
        }

        private List<Accessibility> ParseAccessibility(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<Accessibility>();

            return input
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Enum.Parse<Accessibility>(x.Trim()))
                .ToList();
        }

        public async Task<List<ClassRoomDto>> GetAllAsync()
        {
            return await _context.ClassRooms
                .Select(c => new ClassRoomDto
                {
                    Id = c.Id,
                    NameGrade = c.NameGrade,
                    Floor = c.Floor,
                    Capacity = c.Capacity,
                    Accessibility = c.Accessibility.Select(a => a.ToString()).ToList()
                })
                .ToListAsync();
        }

        public async Task<ClassRoomDto?> GetByIdAsync(Guid id)
        {
            var c = await _context.ClassRooms.FindAsync(id);
            if (c == null) return null;

            return new ClassRoomDto
            {
                Id = c.Id,
                NameGrade = c.NameGrade,
                Floor = c.Floor,
                Capacity = c.Capacity,
                Accessibility = c.Accessibility.Select(a => a.ToString()).ToList()
            };
        }

        public async Task<ClassRoomDto> CreateAsync(CreateClassRoomRequest req)
        {
            var entity = new ClassRoom
            {
                Id = Guid.NewGuid(),
                NameGrade = req.NameGrade,
                Floor = req.Floor,
                Capacity = req.Capacity,
                Accessibility = req.Accessibility
            };

            _context.ClassRooms.Add(entity);
            await _context.SaveChangesAsync();

            return new ClassRoomDto
            {
                Id = entity.Id,
                NameGrade = entity.NameGrade,
                Floor = entity.Floor,
                Capacity = entity.Capacity,
                Accessibility = entity.Accessibility
    .Select(a => a.ToString())
    .ToList()
            };
        }

        public async Task<ClassRoomDto?> UpdateAsync(Guid id, UpdateClassRoomRequest req)
        {
            var entity = await _context.ClassRooms.FindAsync(id);
            if (entity == null) return null;

            if (req.NameGrade != null) entity.NameGrade = req.NameGrade;
            if (req.Floor.HasValue) entity.Floor = req.Floor.Value;
            if (req.Capacity.HasValue) entity.Capacity = req.Capacity.Value;

            if (req.Accessibility != null)
                entity.Accessibility = req.Accessibility;
            await _context.SaveChangesAsync();

            return new ClassRoomDto
            {
                Id = entity.Id,
                NameGrade = entity.NameGrade,
                Floor = entity.Floor,
                Capacity = entity.Capacity,
                Accessibility = entity.Accessibility
    .Select(a => a.ToString())
    .ToList()
            };
        }
    }
}