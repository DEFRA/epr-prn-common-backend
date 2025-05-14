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
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the reprocessing site address
    /// </summary>
    public AddressDto ReprocessingSiteAddress { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the legal document address
    /// </summary>
    public AddressDto LegalDocumentAddress { get; set; }
}
