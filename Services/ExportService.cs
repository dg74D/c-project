using System.Text;
using Server.Services.Interfaces;
using Server.Models;

namespace Server.Services
{
public class ExportService : IExportService
{
    public byte[] ExportAssignmentsCsv(List<Assignment> data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("StudentId,ClassRoomId,Year");

        foreach (var a in data)
        {
            sb.AppendLine($"{a.StudentId},{a.ClassRoomId},{a.Year}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}}