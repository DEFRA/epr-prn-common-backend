using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationOverviewDto
{
    public int Id { get; set; }

    public required string OrganisationName { get; set; } = string.Empty;

    public string? SiteAddress { get; init; }

    public ApplicationOrganisationType OrganisationType { get; set; }

    public required string Regulator { get; set; }

    public List<RegistrationTaskDto> Tasks { get; set; } = [];

    public List<RegistrationMaterialDto> Materials { get; set; } = [];
}