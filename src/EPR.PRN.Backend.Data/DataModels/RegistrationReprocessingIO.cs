using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [MaxLength(2000)]
        public required string TypeOfSuppliers { get; set; }

        public bool ReprocessingPackgagingWasteLastYearFlag { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double UKPackgagingWasteTonne { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double NonUKPackgagingWasteTonne { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double NotUKPackgagingWasteTonne { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double SentToOtherSiteTonne { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double ContaminantsTonne { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double ProcessLossTonne { get; set; }

        public required string PlantEquipmentUsed { get; set; }

        public virtual Registration Registration { get; set; } = null!; 
    }
}
