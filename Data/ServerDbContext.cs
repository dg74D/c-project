using Microsoft.EntityFrameworkCore;
using Server.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Server.Models.DTOs;
using Server.Models;


namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<User> Users => Set<User>();
        public DbSet<History> Histories => Set<History>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Assignments)
                .HasForeignKey(a => a.StudentId);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.ClassRoom)
                .WithMany()
                .HasForeignKey(a => a.ClassRoomId);

            modelBuilder.Entity<History>()
                .HasOne(h => h.User)
                .WithMany(u => u.Histories)
                .HasForeignKey(h => h.UserId);

            var converter = new ValueConverter<List<Accessibility>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<Accessibility>>(v, (JsonSerializerOptions?)null) ?? new List<Accessibility>()
            );

            modelBuilder.Entity<Student>()
                .Property(s => s.Accessibility)
                .HasConversion(converter);

            modelBuilder.Entity<ClassRoom>()
                .Property(x => x.Accessibility)
                .HasConversion(converter);
        }


       
    }
}