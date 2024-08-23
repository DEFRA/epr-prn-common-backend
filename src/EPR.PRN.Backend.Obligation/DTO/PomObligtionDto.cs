#nullable disable

namespace EPR.PRN.Backend.Obligation.DTO;

public class PomObligtionDto
{
    public string SubmissionPeriod { get; set; }
    public string PackagingMaterial { get; set; }
    public double PackagingMaterialWeight { get; set; }
    public int OrganisationId { get; set; }
}
