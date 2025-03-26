using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.ReadModels
{
    [ExcludeFromCodeCoverage]
    public class StatusReadModel
    {
        public Guid PrnId { get; set; }

        public required EprnStatus Status { get; set; }
    }
}