using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetMaterialsByIdHandler(IRegistrationMaterialRepository rmRepository) : IRequestHandler<GetMaterialDetailByIdQuery, RegistrationMaterialDetailsDto>
{
    public async Task<RegistrationMaterialDetailsDto> Handle(GetMaterialDetailByIdQuery request, CancellationToken cancellationToken)
    {
        RegistrationMaterialDetailsDto result = await rmRepository.GetRegistrationMaterialDetailsById(request.Id);
        return result;
    }
}