using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class RegistrationReprocessingIO
    {
        [Key]
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        public string? TypeOfSupplier { get; set; }
        public string? PlantEquipmentUsed { get; set; }
        public bool ReprocessingPackagingWasteLastYearFlag { get; set; }
        public decimal UKPackagingWasteTonne { get; set; }
        public decimal NonUKPackagingWasteTonne { get; set; }
        public decimal NotPackingWasteTonne { get; set; }
        public decimal SenttoOtherSiteTonne { get; set; }
        public decimal ContaminantsTonne { get; set; }
        public decimal ProcessLossTonne { get; set; }
        public decimal TotalInput { get; set; }
        public decimal TotalOutput { get; set; }

    }
}