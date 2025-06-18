using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetAccreditationBusinessPlanQueryHandler(
    IRegulatorAccreditationRepository aRepository,
    IMapper mapper
) : IRequestHandler<GetRegistrationAccreditationBusinessPlanByIdQuery, AccreditationBusinessPlanDto>
{
    public async Task<AccreditationBusinessPlanDto> Handle(GetRegistrationAccreditationBusinessPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var accreditation = await aRepository.GetAccreditationPaymentFeesById(request.Id);
        var accreditationBusinessPlanDto = mapper.Map<AccreditationBusinessPlanDto>(accreditation);
        return accreditationBusinessPlanDto;
    }
}
