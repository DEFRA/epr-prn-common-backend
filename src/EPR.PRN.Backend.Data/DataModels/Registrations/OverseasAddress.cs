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
        public required RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterialId")]
        public int RegistrationMaterialId { get; set; }
        [MaxLength(100)]
        public required string OrganisationName { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public required LookupCountry Country { get; set; }
        [MaxLength(100)]
        public required string AddressLine1 { get; set; }
        [MaxLength(100)]
        public required string AddressLine2 { get; set; }
        [MaxLength(70)]
        public required string CityOrTown { get; set; }
        [MaxLength(70)]
        public required string StateProvince { get; set; }
        [MaxLength(20)]
        public required string PostCode { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public required string SiteCoordinates { get; set; }
    }
}
