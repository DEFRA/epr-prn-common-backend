using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetMaterialPaymentInfoByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialPaymentFeeByIdQuery, MaterialPaymentFeeDto>
{
    public async Task<MaterialPaymentFeeDto> Handle(GetMaterialPaymentFeeByIdQuery request, CancellationToken cancellationToken)
    {
        var registrationMaterial = await rmRepository.GetRegistrationMaterialById(request.Id);
        var materialPaymentFeeDto = mapper.Map<MaterialPaymentFeeDto>(registrationMaterial);

        return materialPaymentFeeDto;
    }
}