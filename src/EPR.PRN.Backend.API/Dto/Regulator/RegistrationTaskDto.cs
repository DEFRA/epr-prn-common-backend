using System.Diagnostics.CodeAnalysis;
namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationTaskDto
{
    public int? Id { get; set; }        // RegulatorRegistrationTaskStatus.Id OR RegulatorApplicationTaskStatus.Id
    public required string TaskName { get; set; }
    public required string Status { get; set; }
   
}

