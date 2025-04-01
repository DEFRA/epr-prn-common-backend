#nullable disable
using EPR.PRN.Backend.API.Dto;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetMaterialDetailByIdQuery : IRequest<RegistrationMaterialDto>
{
    [Required]
    public int Id { get; set; }   

}