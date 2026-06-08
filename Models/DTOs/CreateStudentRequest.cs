
namespace Server.Models.DTOs
{



    public class CreateStudentRequest
    {
        public string FullName { get; set; } = "";
        public string ClassName { get; set; } = ""; // במקום Grade int
        public string Notes { get; set; } = "";
public string Accessibility { get; set; } = "";
    }
}
