using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetMaterialsByIdHandler(IRegistrationMaterialRepository rmRepository) : IRequestHandler<GetMaterialDetailByIdQuery, RegistrationMaterialDto>
{
    public async Task<RegistrationMaterialDto> Handle(GetMaterialDetailByIdQuery request, CancellationToken cancellationToken)
    {

        RegistrationMaterialDto result = await rmRepository.GetMaterialsById(request.Id);
        return result;
    }
}