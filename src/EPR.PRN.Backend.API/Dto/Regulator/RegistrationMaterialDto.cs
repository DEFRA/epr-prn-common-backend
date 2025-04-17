using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

/// <summary>
/// Material-level section inside a registration.
/// </summary>
[ExcludeFromCodeCoverage]
public class RegistrationMaterialDto
{
    public int Id { get; set; } // RegistrationMaterial.Id
    public int RegistrationId { get; set; }
    public required string MaterialName { get; set; }
    public string? Status { get; set; }
    public string? StatusUpdatedBy { get; init; }
    public DateTime? StatusUpdatedDate { get; init; }
    public string? RegistrationReferenceNumber { get; init; }
    public string? Comments { get; set; }
    public DateTime? DeterminationDate { get; set; }
    public List<RegistrationTaskDto> Tasks { get; set; } = [];
}   