using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegistrationsOverviewCommand : IRequest
{
    public int OrganisationId { get; set; }
}
