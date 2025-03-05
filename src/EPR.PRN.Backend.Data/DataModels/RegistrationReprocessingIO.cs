using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationReprocessingIO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        public int MaterialId { get; set; }

        public string? TypeOfSuppliers { get; set; }

        public bool ReprocessingPackgagingWasteLastYearFlag { get; set; }

        public decimal UKPackgagingWasteTonne { get; set; }

        public decimal NonUKPackgagingWasteTonne { get; set; }

        public decimal NotUKPackgagingWasteTonne { get; set; }

        public decimal SentToOtherSiteTonne { get; set; }

        public decimal ContaminantsTonne { get; set; }

        public decimal ProcessLossTonne { get; set; }

        public required string PlantEquipmentUsed { get; set; }
    }
}
