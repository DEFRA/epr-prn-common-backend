using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    [Table("Public.OverseasAddress")]
    public class OverseasAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public Registration? Registration { get; set; }
        [ForeignKey("RegistrationId")]
        public int RegistrationId { get; set; }
        [MaxLength(100)]
        public required string OrganisationName { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public LookupCountry? Country { get; set; }
        [MaxLength(100)]
        public required string AddressLine1 { get; set; }
        [MaxLength(100)]
        public string? AddressLine2 { get; set; }
        [MaxLength(70)]
        public required string CityOrTown { get; set; }
        [MaxLength(70)]
        public string? StateProvince { get; set; }
        [MaxLength(20)]
        public string? PostCode { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? SiteCoordinates { get; set; }
        public List<OverseasAddressContact> OverseasAddressContacts { get; set; } = [];
        public List<OverseasAddressWasteCode> OverseasAddressWasteCodes { get; set; } = [];
        public List<OverseasMaterialReprocessingSite> OverseasMaterialReprocessingSites { get; set; } = [];

        [InverseProperty(nameof(InterimOverseasConnections.ParentOverseasAddress))]
        public List<InterimOverseasConnections> ChildInterimConnections { get; set; } = [];

        
        [InverseProperty(nameof(InterimOverseasConnections.OverseasAddress))]
        public List<InterimOverseasConnections> InterimConnections { get; set; } = [];
    }
}   
