namespace EPR.PRN.Backend.Obligation.Dto
{
    public class ObligationModel
    {
        public int NumberOfPrnsAwaitingAcceptance { get; set; }
        public List<ObligationData> ObligationData { get; set; } = [];
    }
}
