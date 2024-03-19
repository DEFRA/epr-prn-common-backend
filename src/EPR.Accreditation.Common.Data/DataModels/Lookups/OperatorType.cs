using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("OperatorType", Schema = "Lookup")]
    public class OperatorType
    {
        [Key]
        public Enums.OperatorType Id { get; set; }

        [Required]
        [MaxLength(12)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<Accreditation> Accreditations { get; set; }
        #endregion
    }
}
