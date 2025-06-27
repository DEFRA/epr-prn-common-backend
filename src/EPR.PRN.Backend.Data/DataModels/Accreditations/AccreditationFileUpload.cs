using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class AccreditationFileUpload
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public int AccreditationId { get; set; }
    public Guid SubmissionId { get; set; }
    public int? OverseasSiteId { get; set; }

    [MaxLength(50)]
    public required string FileName { get; set; }

    public Guid? FileId { get; set; }
    public DateTime UploadedOn { get; set; }

    [MaxLength(50)]
    public string UploadedBy { get; set; }

    public int FileUploadTypeId { get; set; }
    public int FileUploadStatusId { get; set; }

    public AccreditationEntity Accreditation { get; set; }
    public FileUploadType FileUploadType { get; set; }
    public FileUploadStatus FileUploadStatus { get; set; }
}