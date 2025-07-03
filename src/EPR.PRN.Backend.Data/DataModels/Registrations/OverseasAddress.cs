using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [Table("Public.OverseasAddress")]
    public class OverseasAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterialId")]
        public int RegistrationMaterialId { get; set; }
        [MaxLength(100)]
        public string OrganisationName { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public LookupCountry Country { get; set; }
        [MaxLength(100)]
        public string AddressLine1 { get; set; }
        [MaxLength(100)]
        public string AddressLine2 { get; set; }
        [MaxLength(70)]
        public string CityOrTown { get; set; }
        [MaxLength(70)]
        public string StateProvince { get; set; }
        [MaxLength(20)]
        public string PostCode { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string SiteCoordinates { get; set; }
        public List<OverseasAddressContact> OverseasAddressContacts { get; set; } = [];
        public List<OverseasAddressWasteCode> OverseasAddressWasteCodes { get; set; } = [];
        public List<OverseasMaterialReprocessingSite> OverseasMaterialReprocessingSites { get; set; } = [];
    }
}
