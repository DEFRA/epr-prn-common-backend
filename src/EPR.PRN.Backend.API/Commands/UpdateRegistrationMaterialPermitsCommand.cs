using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

/// <summary>
/// Command to update permit-related information for a registration material permits.
/// </summary>
public sealed class UpdateRegistrationMaterialPermitsCommand : IRequest
{
    /// <summary>
    /// Gets or sets the ID of the registration material. This value is bound from the route or context, not the request body.
    /// </summary>
    [BindNever]
    [SwaggerIgnore]
    public Guid ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the permit type.
    /// </summary>
    public int? PermitTypeId { get; set; }

    /// <summary>
    /// Gets or sets the permit number.
    /// </summary>
    public string? PermitNumber { get; set; }
}
