namespace Server.Models.DTOs
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassRoomId { get; set; }
        public string StudentName { get; set; } = "";
        public string ClassRoomName { get; set; }   = "";
        public int Year { get; set; }
    }
}