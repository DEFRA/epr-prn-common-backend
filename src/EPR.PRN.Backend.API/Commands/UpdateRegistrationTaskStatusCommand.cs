using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateRegistrationTaskStatusCommand : UpdateRegistrationTaskStatusCommandBase, IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid RegistrationId { get; set; }
}