using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

[ExcludeFromCodeCoverage]
public class GetRegistrationTaskOverviewByIdQuery : IRequest<ApplicantRegistrationTasksOverviewDto>
{
    [Required]
    public Guid Id { get; set; }
}