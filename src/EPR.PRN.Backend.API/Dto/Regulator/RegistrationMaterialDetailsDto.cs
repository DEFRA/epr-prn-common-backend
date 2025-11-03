using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialDetailsDto
{
    public Guid Id { get; set; }
    public Guid RegistrationId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public RegistrationMaterialStatus? Status { get; set; }
    public DateTime? DulyMade { get; set; }
    public DateTime? DeterminationDate { get; set; }
}
