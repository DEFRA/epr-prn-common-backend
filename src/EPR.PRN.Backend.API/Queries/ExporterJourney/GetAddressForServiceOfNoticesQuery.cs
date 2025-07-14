using EPR.PRN.Backend.API.Dto.ExporterJourney;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Queries.ExporterJourney;

[ExcludeFromCodeCoverage]
public class GetAddressForServiceOfNoticesQuery : IRequest<GetAddressForServiceOfNoticesDto>
{
    [Required]
    public Guid RegistrationId { get; set; }
}
