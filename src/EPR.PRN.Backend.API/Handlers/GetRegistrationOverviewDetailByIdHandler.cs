using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Repositories.Interfaces;
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