using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.API.Dto.ExporterJourney;

[ExcludeFromCodeCoverage]
public class GetAddressForServiceOfNoticesDto
{
    public Guid RegistrationId { get; set; }

    public required AddressDto? LegalDocumentAddress { get; set; }
}
