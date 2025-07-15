using System.Diagnostics.CodeAnalysis;
namespace EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.DTO
{
    [ExcludeFromCodeCoverage]
    public class OverseasAddressDto
    {
        public Guid ExternalId { get; set; }
        public required string OrganisationName { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string CityOrTown { get; set; }
        public string? StateProvince { get; set; }
        public string? PostCode { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public required string SiteCoordinates { get; set; }
        public required string CountryName { get; set; }
        public List<OverseasAddressContactDto> OverseasAddressContacts { get; set; } = [];
        public List<OverseasAddressWasteCodeDto> OverseasAddressWasteCodes { get; set; } = [];
    }
}
