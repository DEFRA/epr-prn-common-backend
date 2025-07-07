using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto
{
    [ExcludeFromCodeCoverage]
    public class GetMaterialExemptionReferenceDto
    {
        public Guid ExternalId { get; set; }
        public required string ReferenceNo { get; set; }
    }
}
