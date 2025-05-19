using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class AccreditationTaskDto
{
    public int? Id { get; set; }
    public required string TaskName { get; set; }
    public required string Status { get; set; }
   
}

