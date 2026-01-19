using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Common.Dto
{
    [ExcludeFromCodeCoverage]
    public class PrnUpdateStatusDto
    {
        public Guid PrnId { get; set; }

        public required EprnStatus Status { get; set; }

        public string? ObligationYear { get; set; }
    }
}
