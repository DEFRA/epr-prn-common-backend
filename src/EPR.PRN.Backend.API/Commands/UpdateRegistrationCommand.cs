using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class UpdateRegistrationCommand : IRequest
{
    /// <summary>
    /// The unique identifier for the registration that is going to be updated.
    /// </summary>
    [BindNever]
    [SwaggerIgnore]
    public int RegistrationId { get; set; }

    /// <summary>
    /// Gets or sets the business address
    /// </summary>
    public AddressDto BusinessAddress { get; set; } = new();

    /// <summary>
    /// Gets or sets the reprocessing site address
    /// </summary>
    public AddressDto ReprocessingSiteAddress { get; set; } = new();

    /// <summary>
    /// Gets or sets the legal address
    /// </summary>
    public AddressDto LegalAddress { get; set; } = new();
}