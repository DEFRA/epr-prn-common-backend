using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.Dto
{
    [ExcludeFromCodeCoverage]
    public class PrnUpdateStatusDto
    {
        public Guid PrnId { get; set; }

        public required EprnStatus Status { get; set; }

        public string? ObligationYear { get; set; }

        // When set, the update applies only while the PRN is still in this status.
        // A PRN in any other status makes the whole request a conflict.
        public EprnStatus? ExpectedStatus { get; set; }
    }
}
