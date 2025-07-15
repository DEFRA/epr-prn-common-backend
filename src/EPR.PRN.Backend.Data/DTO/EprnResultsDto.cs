using EPR.PRN.Backend.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.Dto
{
    [ExcludeFromCodeCoverage]
    public class EprnResultsDto
    {
        public required Eprn Eprn { get; set; }
        public required PrnStatus Status { get; set; }
    }
}
