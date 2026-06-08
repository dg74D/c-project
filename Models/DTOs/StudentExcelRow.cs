namespace Server.Models.DTOs
{
    public class StudentExcelRow
{
    public string FullName { get; set; }="";
    public int? Grade { get; set; }
    public string Notes { get; set; } = "";
    public string Accessibility { get; set; } = "None"; // טקסט ולא enum =
}
}
