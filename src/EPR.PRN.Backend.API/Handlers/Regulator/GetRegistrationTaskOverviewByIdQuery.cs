using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

[ExcludeFromCodeCoverage]
public class GetRegistrationTaskOverviewByIdQuery : IRequest<RegistrationTaskOverviewDto>
{
    [Required]
    public Guid Id { get; set; }
}