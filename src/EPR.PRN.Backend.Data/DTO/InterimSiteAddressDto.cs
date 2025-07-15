namespace EPR.PRN.Backend.Data.DTO;

public class InterimSiteAddressDto : OverseasAddressBaseDto
{
    public Guid? Id { get; init; }
    public List<OverseasAddressContactDto> InterimAddressContact { get; set; } = new();
}