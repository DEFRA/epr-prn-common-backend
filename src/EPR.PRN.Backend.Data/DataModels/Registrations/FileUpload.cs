using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [Table("public.FileUpload")]
    [ExcludeFromCodeCoverage]
    public class FileUpload
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int? RegistrationMaterialId { get; set; }
        [MaxLength(50)]
        public string? Filename { get; set; }
        public Guid FileId { get; set; }
        public DateTime? DateUploaded { get; set; }
        public Guid UpdatedBy { get; set; }
        public LookupFileUploadType FileUploadType { get; set; }
        [ForeignKey("FileUploadType")]
        public int? FileUploadTypeId { get; set; }
        public LookupFileUploadStatus FileUploadStatus { get; set; }
        [ForeignKey("FileUploadStatus")]
        public int? FileUploadStatusId { get; set; }
        [MaxLength(500)]
        public string? Comments { get; set; }
    }
}