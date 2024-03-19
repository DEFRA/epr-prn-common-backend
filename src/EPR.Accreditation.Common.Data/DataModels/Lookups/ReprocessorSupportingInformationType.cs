using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("ReprocessorSupportingInformationType", Schema = "Lookup")]
    public class ReprocessorSupportingInformationType
    {
        [Key]
        public Enums.ReprocessorSupportingInformationType Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<ReprocessorSupportingInformation> ReprocessorSupportingInformations { get; set; }
        #endregion
    }
}
