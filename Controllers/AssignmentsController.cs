using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/assignments")]
    public class AssignmentsController : ControllerBase
    {
        private readonly AllocationService _service;
        private readonly AppDbContext _context;

        public AssignmentsController(AllocationService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost("run/{year}")]
        public async Task<IActionResult> Run(int year)
        {
            var result = await _service.AllocateAsync(year);
            return Ok(result);
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            var data = await _context.Assignments
                .Where(x => x.Year == year)
                .Include(x => x.Student)
                .Include(x => x.ClassRoom)
                .ToListAsync();

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Assignments.FindAsync(id);
            if (item == null) return NotFound();

            _context.Assignments.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}