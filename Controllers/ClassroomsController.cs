using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Server.Data;
using Server.Models;
using Server.Models.DTOs;
using Server.Services;


namespace Server.Controllers
{
    [ApiController]
    [Route("api/classrooms")]
    public class ClassRoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassRoomsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.ClassRooms.ToListAsync();
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClassRoomRequest req)
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

            var dto = new ClassRoomDto
            {
                Id = entity.Id,
                NameGrade = entity.NameGrade,
                Floor = entity.Floor,
                Capacity = entity.Capacity,

                Accessibility = entity.Accessibility
    .Select(a => a.ToString())
    .ToList()
            };

            return Ok(dto);
        }
        // =========================
        // GET BY ID
        // =========================


        // =========================
        // CREATE
        // =========================


        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ClassRoom req)
        {
            var item = await _context.ClassRooms.FindAsync(id);
            if (item == null) return NotFound();

            item.NameGrade = req.NameGrade;
            item.Floor = req.Floor;
            item.Capacity = req.Capacity;
            item.Accessibility = req.Accessibility;

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.ClassRooms.FindAsync(id);
            if (item == null) return NotFound();

            _context.ClassRooms.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // =========================
        // IMPORT EXCEL
        // =========================
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var list = new List<ClassRoom>();
            var result = new ImportResult();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.FirstOrDefault();

            if (ws == null || ws.Dimension == null)
                return BadRequest("Invalid Excel file");

            string Norm(string s)
                => string.IsNullOrWhiteSpace(s)
                    ? ""
                    : s.Trim().ToLower().Replace(" ", "");

            var cols = new Dictionary<string, int>();

            for (int c = 1; c <= ws.Dimension.Columns; c++)
            {
                var key = Norm(ws.Cells[1, c].Text);
                if (!string.IsNullOrEmpty(key))
                    cols[key] = c;
            }

            string Get(int row, params string[] names)
            {
                foreach (var n in names)
                {
                    var key = Norm(n);
                    if (cols.TryGetValue(key, out int col))
                        return ws.Cells[row, col].Text?.Trim();
                }
                return null;
            }

            for (int r = 2; r <= ws.Dimension.Rows; r++)
            {
                var nameGrade = Get(r, "namegrade", "כיתה", "שם כיתה");
                var floorText = Get(r, "floor", "קומה");
                var capacityText = Get(r, "capacity", "קיבולת", "מקומות");
                var accessibilityText = Get(r, "accessibility", "נגישות");

                if (string.IsNullOrWhiteSpace(nameGrade))
                {
                    result.Errors.Add($"Row {r}: NameGrade is required");
                    continue;
                }

                if (!int.TryParse(floorText, out int floor))
                {
                    result.Errors.Add($"Row {r}: Invalid Floor");
                    continue;
                }

                int.TryParse(capacityText, out int capacity);

                list.Add(new ClassRoom
                {
                    Id = Guid.NewGuid(),
                    NameGrade = nameGrade,
                    Floor = floor,
                    Capacity = capacity,

                    Accessibility = accessibilityText?
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => AccessibilityMapper.Map(x.Trim()))
        .ToList() ?? new List<Accessibility>()
                });
            }

            if (list.Any())
            {
                _context.ClassRooms.AddRange(list);
                await _context.SaveChangesAsync();
            }

            result.SuccessCount = list.Count;

            return Ok(result);
        }

        public class ImportResult
        {
            public int SuccessCount { get; set; }
            public List<string> Errors { get; set; } = new();
        }
    }
}