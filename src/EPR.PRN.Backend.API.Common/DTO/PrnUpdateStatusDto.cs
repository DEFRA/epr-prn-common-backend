using EPR.PRN.Backend.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.DTO
{
    [ExcludeFromCodeCoverage]
    public class PrnUpdateStatusDto
    {
        public Guid PrnId { get; set; }

        public required PrnStatusEnum Status { get; set; }
    }
}
