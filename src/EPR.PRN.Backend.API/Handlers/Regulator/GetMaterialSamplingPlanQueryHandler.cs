using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetMaterialSamplingPlanQueryHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialSamplingPlanQuery, RegistrationMaterialSamplingPlanDto>
{
    public async Task<RegistrationMaterialSamplingPlanDto> Handle(GetMaterialSamplingPlanQuery request, CancellationToken cancellationToken)
    {
        var registrationMaterial = await rmRepository.GetRegistrationMaterialById(request.Id);
        var materialDto = mapper.Map<RegistrationMaterialSamplingPlanDto>(registrationMaterial);
        return materialDto;
    }
}