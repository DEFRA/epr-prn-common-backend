using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class ReprocessorSupportingInformation : IdBaseEntity
    {
        [ForeignKey("MaterialReprocessorDetails")]
        public int MaterialReprocessorDetailsId { get; set; }

        [ForeignKey("ReprocessorSupportingInformationType")]
        public Enums.ReprocessorSupportingInformationType ReprocessorSupportingInformationTypeId { get; set; }

        [MaxLength(20)]
        public string Type {  get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal Tonnes { get; set; }

        #region Navigation properties
        public virtual ReprocessorSupportingInformationType ReprocessorSupportingInformationType { get; set; }

        public virtual MaterialReprocessorDetails MaterialReprocessorDetails { get; set; }
        #endregion
    }
}