#nullable disable

namespace EPR.PRN.Backend.Obligation.Models;

public class SubmissionCalculationRequest
{
    public Guid OrganisationId { get; set; }
    public string SubmissionPeriod { get; set; }
    public string PackagingMaterial { get; set; }
    public int PackagingMaterialWeight { get; set; }
    public Guid SubmitterId { get; set; }
    public string SubmitterType { get; set; }
}
