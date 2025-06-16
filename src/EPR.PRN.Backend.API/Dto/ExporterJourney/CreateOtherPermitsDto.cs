using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.ExporterJourney;

[ExcludeFromCodeCoverage]
public class CreateOtherPermitsDto
{
    public string? WasteLicenseOrPermitNumber { get; set; }

    public string? PpcNumber { get; set; }

    public List<string>? WasteExemptionReference { get; set; }
}
