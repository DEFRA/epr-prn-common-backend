namespace EPR.PRN.Backend.Data.DTO
{
    public class EprnResultsDto
    {
        public required string MaterialName { get; set; }
        public required string StatusName { get; set; }
        public int TotalTonnage { get; set; }
    }
}
