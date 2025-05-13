using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Accreditation
{
    [ExcludeFromCodeCoverage]
    public class AccreditationPrnIssueAuthDto
    {
        public Guid ExternalId { get; set; }
        public Guid AccreditationExternalId { get; set; }
        public int PersonId { get; set; }
    }
}
