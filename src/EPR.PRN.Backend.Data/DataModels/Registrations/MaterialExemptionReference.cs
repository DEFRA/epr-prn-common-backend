using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [Table("Public.MaterialExemptionReference")]
    [ExcludeFromCodeCoverage]
    public class MaterialExemptionReference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; } = null!;
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        [MaxLength(100)]
        public required string ReferenceNo { get; set; }
    }
}