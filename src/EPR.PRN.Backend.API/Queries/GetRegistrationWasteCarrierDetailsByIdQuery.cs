using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetRegistrationWasteCarrierDetailsByIdQuery : IRequest<RegistrationWasteCarrierDto>
{
    [Required]
    public Guid Id { get; set; }
}