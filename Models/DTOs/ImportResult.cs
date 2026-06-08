namespace Server.Models.DTOs
{
    public class ImportResult
    {
        public int SuccessCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}