using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialsAuthorisedOnSiteDto:NoteBase
{
    public Guid RegistrationId { get; set; }
    public string SiteAddress { get; init; } = string.Empty;
    public Guid RegulatorRegistrationTaskStatusId { get; set; }
    public List<MaterialsAuthorisedOnSiteInfoDto> MaterialsAuthorisation { get; set; } = [];   
}