using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("FileUploadStatus", Schema = "Lookup")]
    public class FileUploadStatus
    {
        [Key]
        public Enums.FileUploadStatus Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        #region Navigation properties
        public virtual ICollection<FileUpload> FileUploads { get; set; }
        #endregion
    }
}
