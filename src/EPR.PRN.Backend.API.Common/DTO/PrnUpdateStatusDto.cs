using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.Dto
{
    [ExcludeFromCodeCoverage]
    public class PrnUpdateStatusDto
    {
        public Guid PrnId { get; set; }

        public required EprnStatus Status { get; set; }
    }
}
