using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Queries;

/// <summary>
/// The query for getting a registration by the organisation.
/// </summary>
[ExcludeFromCodeCoverage]
public class GetRegistrationByOrganisationQuery : IRequest<RegistrationDto?>
{
    /// <summary>
    /// The id for the type of application i.e. Reprocessor, Producer, etc.
    /// </summary>
    public int ApplicationTypeId { get; set; }

    /// <summary>
    /// The id for the organisation that is registering.
    /// </summary>
    public Guid OrganisationId { get; set; }
}