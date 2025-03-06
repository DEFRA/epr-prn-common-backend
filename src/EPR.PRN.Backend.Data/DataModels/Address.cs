using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<Registration> Registrations { get; set; }

    }
}