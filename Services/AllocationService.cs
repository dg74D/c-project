using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Services
{
    public class AllocationService
    {
        private readonly AppDbContext _context;

        public AllocationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> AllocateAsync(int year)
        {
            var students = await _context.Students.ToListAsync();
            var classes = await _context.ClassRooms.ToListAsync();

            var result = new List<Assignment>();

            var classCount = classes.ToDictionary(c => c.Id, c => 0);

            foreach (var student in students)
            {
                var previous = await _context.Assignments
                    .Where(a => a.StudentId == student.Id && a.Year == year - 1)
                    .Select(a => a.ClassRoomId)
                    .ToListAsync();

                var room = classes.FirstOrDefault(c =>
                    classCount[c.Id] < c.Capacity &&
                    c.Accessibility == student.Accessibility &&
                    !previous.Contains(c.Id));

                if (room == null) continue;

                classCount[room.Id]++;

                result.Add(new Assignment
                {
                    Id = Guid.NewGuid(),
                    StudentId = student.Id,
                    ClassRoomId = room.Id,
                    Year = year
                });
            }

            _context.Assignments.AddRange(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}