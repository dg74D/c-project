using System.ComponentModel.DataAnnotations;
namespace Server.Models
{
    public class History
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = new User();

        public string ActionType { get; set; } = "";

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Details { get; set; } = "";
    }
}