namespace EPR.PRN.Backend.Obligation.Dto
{
    public class ObligationData
    {
        public Guid OrganisationId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public double Tonnage { get; set; }
        public double MaterialTarget { get; set; }
        public int? ObligationToMeet { get; set; }
        public int TonnageAwaitingAcceptance { get; set; }
        public int TonnageAccepted { get; set; }
        public int? TonnageOutstanding { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
