using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationProcessingIORawMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationReprocessingIOId { get; set; }

        public string? RawMaterialName { get; set; }

        public decimal TonneValue { get; set; }

        public bool IsInput { get; set; }

    }
}
