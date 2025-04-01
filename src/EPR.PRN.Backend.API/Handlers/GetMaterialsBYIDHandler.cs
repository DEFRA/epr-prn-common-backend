using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class GetMaterialsBYIDHandler : IRequestHandler<GetMaterialDetailByIdQuery, RegistrationMaterialDto>
{
    private readonly IRegistrationMaterialRepository _rmRepository;
    public GetMaterialsBYIDHandler(IRegistrationMaterialRepository rmRepository)
    {
        _rmRepository = rmRepository;
    }

    public async Task<RegistrationMaterialDto> Handle(GetMaterialDetailByIdQuery request, CancellationToken cancellationToken)
    {

        RegistrationMaterialDto result = await _rmRepository.GetMaterialsById(request.Id);
        return result;
    }
}