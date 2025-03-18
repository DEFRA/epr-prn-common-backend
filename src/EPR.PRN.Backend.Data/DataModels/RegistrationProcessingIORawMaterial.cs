using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationProcessingIORawMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationReprocessingIOId { get; set; }

        public string? RawMaterialName { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double TonneValue { get; set; }

        public bool IsInput { get; set; }

        public virtual RegistrationReprocessingIO RegistrationReprocessingIO { get; set; }

    }
}
