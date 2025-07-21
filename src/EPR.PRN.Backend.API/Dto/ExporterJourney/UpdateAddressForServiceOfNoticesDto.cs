using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.API.Dto.ExporterJourney;

[ExcludeFromCodeCoverage]
public class UpsertAddressForServiceOfNoticesDto
{
    public AddressDto LegalDocumentAddress { get; set; }
}
