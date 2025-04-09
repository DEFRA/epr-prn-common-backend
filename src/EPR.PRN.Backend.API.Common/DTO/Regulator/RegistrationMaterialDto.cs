using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Common.Dto.Regulator;

/// <summary>
/// Material-level section inside a registration.
/// </summary>
public class RegistrationMaterialDto
{
    public int Id { get; set; } // RegistrationMaterial.Id
    public int RegistrationId { get; set; }
    public required string MaterialName { get; set; }
    public RegistrationMaterialStatus? Status { get; set; }
    public string? StatusUpdatedByName { get; init; }
    public DateTime? StatusUpdatedAt { get; init; }
    public string? RegistrationReferenceNumber { get; init; }
    public string? Comments { get; set; }
    public DateTime? DeterminationDate { get; set; }
    public List<RegistrationTaskDto> Tasks { get; set; } = [];
}   