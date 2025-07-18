using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DTO;

public class OverseasAddressBaseDto
{
    [MaxLength(100)]
    public required string AddressLine1 { get; set; }
    [MaxLength(100)]
    public required string AddressLine2 { get; set; }
    [MaxLength(70)]
    public required string CityOrTown { get; set; }
    public required string CountryName { get; set; }
    public Guid ExternalId { get; set; }
    public required string OrganisationName { get; set; }
    [MaxLength(20)]
    public required string PostCode { get; set; }
    [MaxLength(70)]
    public string? StateProvince { get; set; }
}
