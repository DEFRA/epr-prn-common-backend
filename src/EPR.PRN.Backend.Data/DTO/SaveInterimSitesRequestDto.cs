using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO;

[ExcludeFromCodeCoverage]
public class SaveInterimSitesRequestDto
{
    public Guid RegistrationMaterialId { get; set; }
    public required List<OverseasMaterialReprocessingSiteDto> OverseasMaterialReprocessingSites { get; set; }
    public Guid? UserId { get; set; }
}
