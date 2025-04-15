#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using EPR.PRN.Backend.API.Dto.Regulator;

using MediatR;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetRegistrationOverviewDetailByIdQuery : IRequest<RegistrationOverviewDto>
{
    [Required]
    public int Id { get; set; }
}