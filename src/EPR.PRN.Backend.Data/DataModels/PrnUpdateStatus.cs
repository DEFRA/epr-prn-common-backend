namespace EPR.PRN.Backend.Data.DataModels
{
    public class PrnUpdateStatus
    {
        public required string EvidenceNo { get; set; }
        public required string EvidenceStatusCode { get; set; }
        public DateTime? StatusDate { get; set; }
        public required string AccreditationYear { get; set; }
        public string? SourceSystemId { get; set; }
    }
}
