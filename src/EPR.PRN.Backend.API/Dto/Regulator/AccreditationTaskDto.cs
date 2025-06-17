using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class AccreditationTaskDto
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string TaskName { get; set; }
    public string Status { get; set; }
    public string? Year { get; set; }                               // Optional grouping if applicable
}
