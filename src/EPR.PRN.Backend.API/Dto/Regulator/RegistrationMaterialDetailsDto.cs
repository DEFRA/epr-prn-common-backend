using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialDetailsDto
{
    public int Id { get; set; }
    public int RegistrationId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public RegistrationMaterialStatus? Status { get; set; }
}
