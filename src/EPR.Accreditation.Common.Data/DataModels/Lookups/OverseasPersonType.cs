using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("OverseasPersonType", Schema = "Lookup")]
    public class OverseasPersonType
    {
        [Key]
        public Enums.OverseasPersonType Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<OverseasContactPerson> OverseasContactPeople { get; set; }
        #endregion
    }
}
