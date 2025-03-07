using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class FileUploadStatus
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Name { get; set; }

        public virtual ICollection<FileUpload> FileUploads { get; set; } = null!;
    }
}
