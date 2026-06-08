namespace Server.Models.DTOs
{
    public class ClassRoomDto
    {
        public Guid Id { get; set; }
        public string NameGrade { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }

        // ✅ תמיד רשימה
        public List<string> Accessibility { get; set; } = new();
    }
}