using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("TaskName", Schema = "Lookup")]
    public class TaskName
    {
        [Key]
        public Enums.TaskName Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<AccreditationTaskProgress> TaskProgress { get; set; }
        #endregion
    }
}
