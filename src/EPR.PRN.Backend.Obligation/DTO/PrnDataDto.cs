namespace EPR.PRN.Backend.Obligation.DTO
{
    public class PrnDataDto
    {
        public Guid OrganisationId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public int? ObligationToMeet { get; set; }
        public int TonnageAwaitingAcceptance { get; set; }
        public int TonnageAccepted { get; set; }
        public int? TonnageOutstanding { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
