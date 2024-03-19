using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class AccreditationTaskProgress : IdBaseEntity
    {
        [ForeignKey("TaskStatus")]
        public Enums.TaskStatus TaskStatusId { get; set; }

        [ForeignKey("TaskName")]
        public Enums.TaskName TaskNameId { get; set; }

        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; }

        #region Navigation properties
        public virtual Accreditation Accreditation { get; set; }

        public virtual Lookups.TaskName TaskName { get; set; }

        public virtual Lookups.TaskStatus TaskStatus { get; set; }

        public virtual ICollection<AccreditationTaskProgressMaterial> AccreditationTaskProgressMaterials { get; set; }
        #endregion
    }
}