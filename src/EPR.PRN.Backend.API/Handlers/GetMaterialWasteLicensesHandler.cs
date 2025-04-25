using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetMaterialWasteLicensesQueryHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialWasteLicensesQuery, RegistrationMaterialWasteLicensesDto>
{
    public async Task<RegistrationMaterialWasteLicensesDto> Handle(GetMaterialWasteLicensesQuery request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterial_WasteLicensesById(request.Id);
        var materialDto = mapper.Map<RegistrationMaterialWasteLicensesDto>(materialEntity);
        return materialDto;
    }
}