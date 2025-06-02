using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetRegistrationAccreditationPaymentFeesByIdHandler(
    IRegulatorRegistrationAccreditationRepository repo,
    IMapper mapper
) : IRequestHandler<GetRegistrationAccreditationPaymentFeesByIdQuery, AccreditationPaymentFeeDetailsDto>
{
    public async Task<AccreditationPaymentFeeDetailsDto> Handle(GetRegistrationAccreditationPaymentFeesByIdQuery request, CancellationToken cancellationToken)
    {
        var accreditationEntity = await repo.GetAccreditationPaymentFeesById(request.Id);
        var accreditationDto = mapper.Map<AccreditationPaymentFeeDetailsDto>(accreditationEntity);
        return accreditationDto;    
    }
}