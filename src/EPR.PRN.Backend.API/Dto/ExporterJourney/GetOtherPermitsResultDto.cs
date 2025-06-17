using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.ExporterJourney;

[ExcludeFromCodeCoverage]
public class GetOtherPermitsResultDto
{
    public Guid Id { get; set; }

    public Guid RegistrationId { get; set; }

    public string? WasteLicenseOrPermitNumber { get; set; }

    public string? PpcNumber { get; set; }

    public List<string> WasteExemptionReference { get; set; } = new List<string>();
}
