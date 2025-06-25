using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.ExporterJourney;

[ExcludeFromCodeCoverage]
public class UpdateCarrierBrokerDealerPermitsDto
{
    [MaxLength(16, ErrorMessage = "WasteCarrierBrokerDealerRegistration must not exceed 16 characters")]
    public string? WasteCarrierBrokerDealerRegistration { get; set; }

    [MaxLength(20, ErrorMessage = "WasteLicenseOrPermitNumber must not exceed 20 characters")]
    public string? WasteLicenseOrPermitNumber { get; set; }

    [MaxLength(20, ErrorMessage = "PpcNumber must not exceed 20 characters")]
    public string? PpcNumber { get; set; }

    public List<string>? WasteExemptionReference { get; set; }
}
