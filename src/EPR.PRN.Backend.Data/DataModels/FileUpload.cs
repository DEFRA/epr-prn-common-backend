using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class FileUpload
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        public string? FileName { get; set; }

        public Guid FileId { get; set; }

        public DateTime DateUploaded { get; set; }

        public Guid UploadedBy { get; set; }

        public int FileUploadTypeId { get; set; }

        public int MaterialId { get; set; }

        public int FileUploadStatusId { get; set; }

        public virtual FileUploadStatus FileUploadStatus { get; set; }

        public virtual FileUploadType FileUploadType { get; set; }

        public virtual Registration Registration { get; set; }
    }
}
