namespace Server.Models.DTOs
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ActionType { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Details { get; set; } = "";
    }
}