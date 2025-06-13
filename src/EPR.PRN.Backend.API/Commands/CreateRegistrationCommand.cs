using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DTO;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationCommand : IRequest<int>
{
    /// <summary>
    /// The ID for the type of application i.e. Producer, Reprocessor, etc.
    /// </summary>
    public int ApplicationTypeId { get; set; }
    /// <summary>
    /// The unique organisation id for which the registration is being created.
    /// </summary>
    public Guid OrganisationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the reprocessing site address
    /// </summary>
    public AddressDto ReprocessingSiteAddress { get; set; } = new();
}
