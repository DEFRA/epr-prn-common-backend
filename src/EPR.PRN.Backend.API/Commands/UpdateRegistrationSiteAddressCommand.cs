using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class UpdateRegistrationSiteAddressCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid RegistrationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the reprocessing site address
    /// </summary>
    public AddressDto ReprocessingSiteAddress { get; set; } = new AddressDto();
}
