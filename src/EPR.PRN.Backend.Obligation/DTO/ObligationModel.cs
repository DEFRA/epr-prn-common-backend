namespace EPR.PRN.Backend.Obligation.DTO
{
    public class ObligationModel
    {
        public int NumberOfPrnsAwaitingAcceptance { get; set; }
        public List<ObligationData> ObligationData { get; set; } = new List<ObligationData>();
    }
}
