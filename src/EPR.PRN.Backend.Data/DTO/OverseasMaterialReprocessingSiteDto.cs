using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO;

[ExcludeFromCodeCoverage]
public class OverseasMaterialReprocessingSiteDto
{
    public Guid Id { get; init; }
    public Guid OverseasAddressId { get; init; }
    public required OverseasAddressBaseDto OverseasAddress { get; init; }
    public List<InterimSiteAddressDto>? InterimSiteAddresses { get; set; } = new();
}