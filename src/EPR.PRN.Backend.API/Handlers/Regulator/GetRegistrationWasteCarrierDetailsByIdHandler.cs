using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetRegistrationWasteCarrierDetailsByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetRegistrationWasteCarrierDetailsByIdQuery, RegistrationWasteCarrierDto>
{
    public async Task<RegistrationWasteCarrierDto> Handle(GetRegistrationWasteCarrierDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await rmRepository.GetRegistrationById(request.Id);
        var wasteCarrierDto = mapper.Map<RegistrationWasteCarrierDto>(registration);

        return wasteCarrierDto;
    }
}