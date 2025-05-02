using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class FileUpload
    {
        [Key]
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public RegistrationMaterial RegistrationMaterial { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        public string? Filename { get; set; }
        public string? FileId { get; set; }
        public DateTime? DateUploaded { get; set; }
        public string? UpdatedBy { get; set; }
        public LookupFileUploadType FileUploadType { get; set; }
        [ForeignKey("FileUploadType")]
        public int? FileUploadTypeId { get; set; }
        public LookupFileUploadStatus FileUploadStatus { get; set; }
        [ForeignKey("FileUploadStatus")]
        public int? FileUploadStatusId { get; set; }
        public string? Comments { get; set; }
    }
}