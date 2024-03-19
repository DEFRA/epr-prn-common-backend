using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class AccreditationTaskProgressMaterial : IdBaseEntity
    {
        [ForeignKey("AccreditationTaskProgress")]
        public int AccreditationTaskProgressId { get; set; }

        [ForeignKey("AccreditationMaterial")]
        public int AccreditationMaterialId { get; set; }

        [ForeignKey("TaskStatus")]
        public TaskStatus TaskStatusId { get; set; }

        #region Navigation properties
        public virtual AccreditationMaterial AccreditationMaterial { get; set; }

        public virtual AccreditationTaskProgress AccreditationTaskProgress { get; set; }
        #endregion
    }
}