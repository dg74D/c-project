

namespace Server.Models.DTOs
{

    public class UpdateStudentRequest
    {
        public string? FullName { get; set; }
        public string? Notes { get; set; }
        public string ClassName { get; set; } = ""; // במקום Grade int
       public List<Accessibility> Accessibility { get; set; }
    }
}
