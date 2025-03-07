using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        [MaxLength(200)]
        public string AddressLine1 { get; set; }

        [MaxLength(200)]
        public string AddressLine2 { get; set; }

        [Required]
        [MaxLength(50)]
        public string TownCity { get; set; }

        [MaxLength(50)]
        public string County { get; set; }

        [Required]
        [MaxLength(10)]
        public string Postcode { get; set; }

        [Required]
        public int NationId { get; set; }

        [Required]
        [MaxLength(20)]
        public string GridReference { get; set; }


        [InverseProperty(nameof(Registration.ReprocessingSiteAddress))]
        public virtual ICollection<Registration> ReprocessingSiteAddresses { get; set; } = null!;

        [InverseProperty(nameof(Registration.BusinessAddress))]
        public virtual ICollection<Registration> BusinessAddresses { get; set; } = null!;

        [InverseProperty(nameof(Registration.LegalDocumentAddress))]
        public virtual ICollection<Registration> LegalDocumentAddresses { get; set; } = null!;

    }
}