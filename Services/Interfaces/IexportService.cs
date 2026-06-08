using Server.Data;
using Server.Models;

namespace Server.Services.Interfaces
{



public interface IExportService
{
    byte[] ExportAssignmentsCsv(List<Assignment> data);
}}