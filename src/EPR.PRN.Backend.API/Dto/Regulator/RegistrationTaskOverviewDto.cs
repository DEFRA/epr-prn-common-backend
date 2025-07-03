using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationTaskOverviewDto
{
    public Guid Id { get; set; }

    public Guid OrganisationId { get; set; } 

    public required string Regulator { get; set; }

    public List<RegistrationTaskDto> Tasks { get; set; } = [];

    public List<RegistrationMaterialDto> Materials { get; set; } = [];
}