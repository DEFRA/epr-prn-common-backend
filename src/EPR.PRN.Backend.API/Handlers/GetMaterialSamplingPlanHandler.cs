using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetMaterialSamplingPlanQueryHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialSamplingPlanQuery, MaterialSamplingPlanDto>
{
    public async Task<MaterialSamplingPlanDto> Handle(GetMaterialSamplingPlanQuery request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.Id);
        var materialDto = mapper.Map<MaterialSamplingPlanDto>(materialEntity);
        return materialDto;
    }
}