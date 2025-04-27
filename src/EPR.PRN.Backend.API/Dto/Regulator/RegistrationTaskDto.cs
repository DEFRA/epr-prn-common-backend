using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
[JsonConverter(typeof(RegistrationTaskDtoConverter))]
public class RegistrationTaskDto
{
    public int? Id { get; set; }        // RegulatorRegistrationTaskStatus.Id OR RegulatorApplicationTaskStatus.Id
    public required string TaskName { get; set; }
    public required string Status { get; set; }
    public RegistrationTaskDataDto? TaskData { get; set; }
}

public abstract class RegistrationTaskDataDto { }

public class SiteAddressAndContactDetailsTaskDataDto : RegistrationTaskDataDto
{
    public int NationId { get; set; }
    public string SiteAddress { get; set; } = string.Empty;
    public string GridReference { get; set; } = string.Empty;
    public string LegalCorrespondenceAddress { get; set; } = string.Empty;
}

// WasteLicensesPermitsAndExemptions task-specific data
public class MaterialsAuthorisedOnSiteTaskDataDto : RegistrationTaskDataDto
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public List<MaterialsAuthorisedOnSiteInfoDto> MaterialsAuthorisation { get; set; } = [];
}

public class MaterialsAuthorisedOnSiteInfoDto
{
    public string Material { get; set; } = string.Empty;
    public string RegistrationStatus { get; set; } = string.Empty;
    public string? Reason { get; set; } 
}