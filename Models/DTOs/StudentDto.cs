using System.ComponentModel.DataAnnotations;

namespace Server.Models.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string ClassName { get; set; } = "";

       
        public string? Notes { get; set; }

       
        public string? Accessibility { get; set; }
    }
}