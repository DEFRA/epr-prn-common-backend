using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetMaterialWasteLicencesQueryHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialWasteLicencesQuery, RegistrationMaterialWasteLicencesDto>
{
    public async Task<RegistrationMaterialWasteLicencesDto> Handle(GetMaterialWasteLicencesQuery request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterial_WasteLicencesById(request.Id);
        var materialDto = mapper.Map<RegistrationMaterialWasteLicencesDto>(materialEntity);
        return materialDto;
    }
}