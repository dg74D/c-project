using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Services.Interfaces;
namespace Server.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _context;

        public AssignmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> GetByYear(int year)
        {
            return await _context.Assignments
                .Include(x => x.Student)
                .Include(x => x.ClassRoom)
                .Where(x => x.Year == year)
                .ToListAsync();
        }
        public async Task<List<Assignment>> GetByYearAsync(int year)
        {
            return await _context.Assignments
                .Include(x => x.Student)
                .Include(x => x.ClassRoom)
                .Where(x => x.Year == year)
                .ToListAsync();
        }
        public async Task<List<Assignment>> RunAlgorithmAsync(int year)
        {
            var students = await _context.Students.ToListAsync();
            var classes = await _context.ClassRooms.ToListAsync();

            var result = new List<Assignment>();

            var classCount = classes.ToDictionary(c => c.Id, c => 0);

            foreach (var student in students)
            {
                var prev = await _context.Assignments
                    .Where(a => a.StudentId == student.Id && a.Year == year - 1)
                    .Select(a => a.ClassRoomId)
                    .ToListAsync();
                var room = classes.FirstOrDefault(c =>
      classCount[c.Id] < c.Capacity &&
      !prev.Contains(c.Id) &&
      (
          !c.Accessibility.Any() ||
          student.Accessibility.Any(a => c.Accessibility.Contains(a))
      )
  );
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