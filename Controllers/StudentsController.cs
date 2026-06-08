using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Server.Data;
using Server.Models;
using Server.Models.DTOs;
using Server.Services;
using Server.Services.Interfaces;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHistoryService _historyService;

        public StudentsController(
            AppDbContext context,
            IHistoryService historyService)
        {
            _context = context;
            _historyService = historyService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _context.Students.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentRequest req)
        {
            var student = new Student
            {
                Id = Guid.NewGuid(),
                FullName = req.FullName,
                ClassName = req.ClassName,
                Notes = req.Notes,
                Accessibility = AccessibilityMapper.Parse(string.Join(",", req.Accessibility))
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            Guid userId;
            var userClaim = User.FindFirst("id")?.Value;
            userId = string.IsNullOrEmpty(userClaim) ? Guid.Empty : Guid.Parse(userClaim);
            await _historyService.AddAsync(
    userId: userId,
    actionType: "Create",
    details: JsonSerializer.Serialize(student)
);

            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateStudentRequest req)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            if (req.FullName != null) student.FullName = req.FullName;
            if (req.ClassName != null) student.ClassName = req.ClassName;
            if (req.Notes != null) student.Notes = req.Notes;
            if (req.Accessibility != null)
                student.Accessibility = AccessibilityMapper.Parse(string.Join(",", req.Accessibility));

            await _context.SaveChangesAsync();
            Guid userId;
            var userClaim = User.FindFirst("id")?.Value;
            userId = string.IsNullOrEmpty(userClaim) ? Guid.Empty : Guid.Parse(userClaim);

            await _historyService.AddAsync(
                userId: userId,
                actionType: "Update", // לא "Create"
                details: JsonSerializer.Serialize(student)
            );
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            Guid userId;
            var userClaim = User.FindFirst("id")?.Value;
            userId = string.IsNullOrEmpty(userClaim) ? Guid.Empty : Guid.Parse(userClaim);

            await _historyService.AddAsync(
                userId: userId,
                actionType: "Delete",
                details: JsonSerializer.Serialize(student)
            );
            return Ok();
        }

        // =========================
        // IMPORT
        // =========================
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var studentsToAdd = new List<Student>();
            var result = new ImportResult();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null || worksheet.Dimension == null)
                return BadRequest("Invalid Excel file");

            string Normalize(string input)
                => string.IsNullOrWhiteSpace(input) ? "" : input.Trim().ToLower().Replace(" ", "");

            var columns = new Dictionary<string, int>();
            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                var header = Normalize(worksheet.Cells[1, col].Text);
                if (!string.IsNullOrWhiteSpace(header))
                    columns[header] = col;
            }

            string GetCell(int row, params string[] names)
            {
                foreach (var name in names)
                {
                    var key = Normalize(name);
                    if (columns.TryGetValue(key, out int col))
                    {
                        var val = worksheet.Cells[row, col].Text?.Trim();
                        if (!string.IsNullOrWhiteSpace(val)) return val;
                    }
                }
                return null;
            }

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var fullName = GetCell(row, "fullname", "שםמלא", "שם מלא", "name");
                var className = GetCell(row, "classname", "class", "כיתה", "classroom");
                var notes = GetCell(row, "notes", "הערות");
                var accessibilityText = GetCell(row, "accessibility", "נגישות");

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    result.Errors.Add($"Row {row}: FullName is required");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(className))
                {
                    result.Errors.Add($"Row {row}: ClassName is required");
                    continue;
                }

                studentsToAdd.Add(new Student
                {
                    Id = Guid.NewGuid(),
                    FullName = fullName,
                    ClassName = className,
                    Notes = notes ?? "",
                    Accessibility = AccessibilityMapper.Parse(accessibilityText)
                });
            }

            if (studentsToAdd.Count > 0)
            {
                _context.Students.AddRange(studentsToAdd);
                await _context.SaveChangesAsync();
                foreach (var student in studentsToAdd)
                {
                    Guid userId;
                    var userClaim = User.FindFirst("id")?.Value;
                    userId = string.IsNullOrEmpty(userClaim) ? Guid.Empty : Guid.Parse(userClaim);

                    await _historyService.AddAsync(
                        userId: userId,
                        actionType: "Import",
                        details: JsonSerializer.Serialize(student)
                    );
                }
            }

            result.SuccessCount = studentsToAdd.Count;
            return Ok(result);
        }

        public class ImportResult
        {
            public int SuccessCount { get; set; }
            public List<string> Errors { get; set; } = new();
        }
    }
}