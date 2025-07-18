namespace EPR.PRN.Backend.Data.DTO;

public class InterimSiteAddressDto : OverseasAddressBaseDto
{
    public List<OverseasAddressContactDto> InterimAddressContact { get; set; } = new();
    public Guid? ParentExternalId { get; set; }
}