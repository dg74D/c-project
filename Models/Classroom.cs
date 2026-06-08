using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class ClassRoom
    {
        public Guid Id { get; set; }
        public string NameGrade { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }

        // public List<Accessibility> Accessibility { get; set; }
        public List<Accessibility> Accessibility { get; set; }
    }
}