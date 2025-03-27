#nullable disable
using EPR.PRN.Backend.API.Models.ReadModel;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetAllMaterialsByIdQuery  : IRequest<RegistrationMaterialTaskReadModel>
{
    [Required]
    public Guid RegistrationID { get; set; }
    

}