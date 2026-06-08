using System.ComponentModel.DataAnnotations;
namespace Server.Models
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; } = "";

        public string ClassName { get; set; } = ""; // במקום Grade int


        public string Notes { get; set; } = "";

        public List<Accessibility> Accessibility { get; set; } = new();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }

}





