using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetMaterialReprocessingIOQueryHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialReprocessingIOQuery, MaterialreprocessingIODto>
{
    public async Task<MaterialreprocessingIODto> Handle(GetMaterialReprocessingIOQuery request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.Id);
        var materialDto = mapper.Map<MaterialreprocessingIODto>(materialEntity);
        return materialDto;
    }
}