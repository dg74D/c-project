using Server.Models.DTOs;
using Server.Models;
using Server.Data;
using Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Server.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync()
        => await _context.Students.ToListAsync();

    public async Task<Student?> GetByIdAsync(Guid id)
        => await _context.Students.FindAsync(id);

    public async Task<Student> CreateAsync(CreateStudentRequest request)
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            ClassName = request.ClassName,
            Notes = request.Notes,
            Accessibility = new List<Accessibility>()
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student?> UpdateAsync(Guid id, UpdateStudentRequest request)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return null;

        if (request.FullName != null) student.FullName = request.FullName;
        if (request.ClassName != null) student.ClassName = request.ClassName;
        if (request.Notes != null) student.Notes = request.Notes;

        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}