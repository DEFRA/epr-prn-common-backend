#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using EPR.PRN.Backend.API.Dto.Regulator;

using MediatR;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetRegistrationAccreditationPaymentFeesByIdQuery : IRequest<AccreditationPaymentFeeDetailsDto>
{
    [Required]
    public Guid Id { get; set; }
}