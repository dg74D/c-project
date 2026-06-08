using System.Collections.Generic;

namespace Server.Models.DTOs
{
    public class UpdateClassRoomRequest
    {
        public string? NameGrade { get; set; }
        public int? Floor { get; set; }
        public int? Capacity { get; set; }

        public List<Accessibility> Accessibility { get; set; }
    }
}

