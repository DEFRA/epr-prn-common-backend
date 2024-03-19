using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("WasteCodeType", Schema = "Lookup")]
    public class WasteCodeType
    {
        [Key]
        public Enums.WasteCodeType Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<WasteCode> WasteCodes { get; set; }
        #endregion
    }
}
