using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class AccreditationMaterial : IdBaseEntity
    {
        public Guid ExternalId { get; set; }

        public int MaterialId { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal AnnualCapacity { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal WeeklyCapacity { get;set; }

        [MaxLength(200)]
        public string WasteSource { get; set; }

        [ForeignKey("Site")]
        public int? SiteId { get; set; } // has a site if the material is for a Reprocessor

        [ForeignKey("OverseasReprocessingSite")]
        public int? OverseasReprocessingSiteId { get; set; } // has an overseas reprocessing site if the material is for an exporter

        #region Navigation properties
        public virtual Site Site { get; set; }

        public virtual OverseasReprocessingSite OverseasReprocessingSite { get; set; }

        public virtual MaterialReprocessorDetails MaterialReprocessorDetails { get; set; }

        public virtual ICollection<AccreditationTaskProgressMaterial> AccreditationTaskProgressMaterials { get; set; }

        public virtual ICollection<WasteCode> WasteCodes { get; set; }
        #endregion
    }
}