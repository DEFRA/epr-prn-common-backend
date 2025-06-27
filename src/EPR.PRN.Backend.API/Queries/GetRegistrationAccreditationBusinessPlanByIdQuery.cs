using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Queries;

public class GetRegistrationAccreditationBusinessPlanByIdQuery : IRequest<AccreditationBusinessPlanDto>
{
    public Guid Id { get; set; }
}