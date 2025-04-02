using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public class RegistrationTaskDto
{
    public int Id { get; set; }        // RegulatorRegistrationTaskStatus.Id OR RegulatorApplicationTaskStatus.Id
    public int TaskId { get; set; }    // Task.Id (lookup)
    public string TaskName { get; set; }
    public string Status { get; set; }
}
