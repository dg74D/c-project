using System.ComponentModel.DataAnnotations;
using Server.Models;
using Server.Models.DTOs;
namespace Server.Models
{
   public class Assignment
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }
    public Student Student { get; set; }

    public Guid ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; }

    public int Year { get; set; }
}public class AssignmentDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid ClassRoomId { get; set; }
    public string StudentName { get; set; }
    public string ClassRoomName { get; set; }
    public int Year { get; set; }
}
}