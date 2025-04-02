using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class GetRegistrationOverviewDetailByIdHandler : IRequestHandler<GetRegistrationOverviewDetailByIdQuery, RegistrationOverviewDto>
{
    private readonly IRegistrationMaterialRepository _rmRepository;
    public GetRegistrationOverviewDetailByIdHandler(IRegistrationMaterialRepository rmRepository)
    {
        _rmRepository = rmRepository;
    }

    public async Task<RegistrationOverviewDto> Handle(GetRegistrationOverviewDetailByIdQuery request, CancellationToken cancellationToken)
    {

        RegistrationOverviewDto result = await _rmRepository.GetRegistrationOverviewDetailById(request.Id);

        return result;
    }

}