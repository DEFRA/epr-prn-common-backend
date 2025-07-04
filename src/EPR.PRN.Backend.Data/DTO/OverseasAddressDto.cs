namespace EPR.PRN.Backend.Data.DTO;

public class OverseasAddressDto
{       
    public Guid ExternalId { get; set; }
    public string OrganisationName { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string CityOrTown { get; set; }
    public string? StateProvince { get; set; }
    public string? PostCode { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
    public string SiteCoordinates { get; set; }
    public string CountryName { get; set; }
    public List<OverseasAddressContactDto> OverseasAddressContacts { get; set; }
    public List<OverseasAddressWasteCodeDto> OverseasAddressWasteCodes { get; set; }
}
