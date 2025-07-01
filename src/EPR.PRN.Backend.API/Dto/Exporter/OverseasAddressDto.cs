namespace EPR.PRN.Backend.API.Dto.Exporter
{
    public class OverseasAddressDto
    {       
        public Guid ExternalId { get; set; }
        public required string OrganisationName { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required string CityOrTown { get; set; }
        public required string StateProvince { get; set; }
        public required string PostCode { get; set; }
        public Guid CreatedBy { get; set; }
        public required string SiteCoordinates { get; set; }
        public required string CountryName { get; set; }
        public List<OverseasAddressContactDto> OverseasAddressContacts { get; set; }
        public List<OverseasAddressWasteCodesDto> OverseasAddressWasteCodes { get; set; }
    }
}
