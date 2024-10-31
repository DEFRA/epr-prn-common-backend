using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.DTO
{
    public class EprnResultsDto
    {
        public required Eprn Eprn { get; set; }
        public required PrnStatus Status { get; set; }
    }
}
