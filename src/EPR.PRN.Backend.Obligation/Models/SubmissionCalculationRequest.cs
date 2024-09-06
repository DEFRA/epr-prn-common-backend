#nullable disable

namespace EPR.PRN.Backend.Obligation.Models;

public class SubmissionCalculationRequest
{
    public string SubmissionPeriod { get; set; }
    public string PackagingMaterial { get; set; }
    public double PackagingMaterialWeight { get; set; }
    public int OrganisationId { get; set; }
}
