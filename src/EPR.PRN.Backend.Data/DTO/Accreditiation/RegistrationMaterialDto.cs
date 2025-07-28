using EPR.PRN.Backend.Data.DTO.Registration;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO.Accreditiation
{
    [ExcludeFromCodeCoverage]
    public class RegistrationMaterialDto
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public int RegistrationId { get; set; }
        public int MaterialId { get; set; }
        public int? StatusId { get; set; }
        public string? RegistrationReferenceNumber { get; set; } = null;
        public string? Comments { get; set; } = string.Empty;
        public DateTime StatusUpdatedDate { get; set; }
        public Guid? StatusUpdatedBy { get; set; }
        public string? ReasonforNotreg { get; set; } = string.Empty;
    }
}