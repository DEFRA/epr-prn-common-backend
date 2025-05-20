using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class MaterialReprocessorDetails : IdBaseEntity
    {
        [ForeignKey("AccreditationMaterial")]
        public int AccreditationMaterialId { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? UkPackagingWaste { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? NonUkPackagingWaste { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? NonPackagingWaste { get;set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? MaterialsNotProcessedOnSite { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? Contaminents {  get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? ProcessLoss { get; set; }

        #region Navigation properties
        public virtual ICollection<ReprocessorSupportingInformation> ReprocessorSupportingInformation { get; set; }
        #endregion
    }
}