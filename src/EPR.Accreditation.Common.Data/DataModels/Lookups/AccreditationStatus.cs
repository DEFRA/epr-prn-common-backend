using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("AccreditationStatus", Schema = "Lookup")]
    public class AccreditationStatus
    {
        [Key]
        public Enums.AccreditationStatus Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<Accreditation> Accreditations { get; set; }
        #endregion
    }
}