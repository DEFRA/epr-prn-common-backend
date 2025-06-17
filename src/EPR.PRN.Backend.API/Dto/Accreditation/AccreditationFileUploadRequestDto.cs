using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Accreditation
{
    [ExcludeFromCodeCoverage]
    public class AccreditationFileUploadRequestDto
    {
        public Guid ExternalId { get; set; }
        public string Filename { get; set; }
        public Guid FileId { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
        public int FileUploadTypeId { get; set; }
        public int MaterialId { get; set; }
        public int FileUploadStatusId { get; set; }
    }
}
