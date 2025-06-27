using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

[ExcludeFromCodeCoverage]
public class GetRegistrationByIdQuery : IRequest<RegistrationOverviewDto>
{
    [Required]
    public Guid Id { get; set; }
}

[ExcludeFromCodeCoverage]
public class GetRegistrationTaskOverviewByIdQuery : IRequest<RegistrationTaskOverviewDto>
{
    [Required]
    public Guid Id { get; set; }
}
