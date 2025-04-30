using AutoMapper;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;

public class GetMaterialsAuthorisedOnSiteByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialsAuthorisedOnSiteByIdQuery, MaterialsAuthorisedOnSiteDto>
{
    public async Task<MaterialsAuthorisedOnSiteDto> Handle(GetMaterialsAuthorisedOnSiteByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await rmRepository.GetRegistrationById(request.Id);
        var materialsAuthorisedDto = mapper.Map<MaterialsAuthorisedOnSiteDto>(registration);
        return materialsAuthorisedDto;
    }
}
