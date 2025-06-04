using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetMaterialDetailByIdQuery : IRequest<RegistrationMaterialDetailsDto>
{
    [Required]
    public int Id { get; set; }
}