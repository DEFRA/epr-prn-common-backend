using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class FileUpload : IdBaseEntity
    {
        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Filename { get; set; }

        public Guid? FileId { get; set; }

        public DateTime DateUploaded {  get; set; }

        [Required]
        [MaxLength(50)]
        public string UploadedBy { get; set; }

        [ForeignKey("FileUploadType")]
        public Enums.FileUploadType FileUploadTypeId { get; set; }

        [ForeignKey("FileUploadStatus")]
        public Enums.FileUploadStatus Status { get; set; }

        #region Navigation properties
        public virtual Accreditation Accreditation { get; set; }

        public virtual FileUploadType FileUploadType { get; set; }

        public virtual FileUploadStatus FileUploadStatus { get; set; }
        #endregion
    }
}
