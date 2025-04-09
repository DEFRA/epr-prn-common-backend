#nullable disable
using EPR.PRN.Backend.API.Common.Dto.Regulator;
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