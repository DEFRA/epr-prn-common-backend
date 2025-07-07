using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

public class UpdateRegistrationMaterialTaskStatusCommand : UpdateRegistrationTaskStatusCommandBase, IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid RegistrationMaterialId { get; set; }
}