using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class MaterialExemptionReference
    {
        [Key]
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        public required string ReferenceNo { get; set; }
    }
}