using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO
{
    [ExcludeFromCodeCoverage]
    public class OverseasAddressWasteCodeDto
    {
        public Guid ExternalId { get; set; }
        public required string CodeName { get; set; }
    }
}
