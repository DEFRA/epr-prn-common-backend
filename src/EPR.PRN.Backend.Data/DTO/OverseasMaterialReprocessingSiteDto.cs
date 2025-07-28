namespace EPR.PRN.Backend.Data.DTO;

public class OverseasMaterialReprocessingSiteDto
{
    public Guid Id { get; init; }
    public Guid OverseasAddressId { get; init; }
    public required OverseasAddressDto OverseasAddress { get; init; }
    public List<InterimSiteAddressDto>? InterimSiteAddresses { get; set; } = new();
}