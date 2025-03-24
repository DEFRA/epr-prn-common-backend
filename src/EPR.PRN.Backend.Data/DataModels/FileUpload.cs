using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class FileUpload
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        [MaxLength(50)]
        public required string FileName { get; set; }

        public Guid FileId { get; set; }

        public DateTime DateUploaded { get; set; }

        [MaxLength(200)]
        public Guid UploadedBy { get; set; }

        public int FileUploadTypeId { get; set; }

        public int MaterialId { get; set; }

        public int FileUploadStatusId { get; set; }

        public virtual FileUploadStatus FileUploadStatus { get; set; }

        public virtual FileUploadType FileUploadType { get; set; }

        public virtual Registration Registration { get; set; }

        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; }
    }
}
