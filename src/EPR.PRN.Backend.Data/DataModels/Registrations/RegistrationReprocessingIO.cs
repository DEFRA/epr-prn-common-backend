using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [Table("Public.RegistrationReprocessingIO")]
    [ExcludeFromCodeCoverage]
    public class RegistrationReprocessingIO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        [MaxLength(2000)]
        public string? TypeOfSupplier { get; set; }
        [MaxLength(2000)]
        public string? PlantEquipmentUsed { get; set; }
        public bool ReprocessingPackagingWasteLastYearFlag { get; set; }
        public decimal UKPackagingWasteTonne { get; set; }
        public decimal NonUKPackagingWasteTonne { get; set; }
        public decimal NotPackingWasteTonne { get; set; }
        public decimal SenttoOtherSiteTonne { get; set; }
        public decimal ContaminantsTonne { get; set; }
        public decimal ProcessLossTonne { get; set; }
        public decimal TotalInputs { get; set; }
        public decimal TotalOutputs { get; set; }
        public List<ReprocessingIORawMaterialorProducts> RawMaterialorProducts { get; set; }
    }
}