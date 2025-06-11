using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationCommand : IRequest<int>
{
    public int ApplicationTypeId { get; set; }
    public Guid OrganisationId { get; set; }
}
