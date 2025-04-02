namespace EPR.PRN.Backend.API.Common.Dto.Regulator;

/// <summary>
/// Material-level section inside a registration.
/// </summary>
public class RegistrationMaterialDto
{
    public int Id { get; set; } // RegistrationMaterial.Id
    public string MaterialName { get; set; }
    public string Status { get; set; }
    public string ReferenceNumber { get; set; }
    public string? Comments { get; set; }
    public DateTime? DeterminationDate { get; set; }
    public List<RegistrationTaskDto> Tasks { get; set; }
}
