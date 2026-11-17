namespace EPR.PRN.Backend.Obligation.Dto;

public class ObligationCalculationRecord
{
    public Guid SubmitterId { get; init; }
    public string SubmitterType { get; init; } = string.Empty;
    public Guid OrganisationId { get; init; }
    public int Year { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public int Tonnage { get; init; }
    public int MaterialObligationValue { get; init; }
    public DateTime CalculatedOn { get; init; }
}
