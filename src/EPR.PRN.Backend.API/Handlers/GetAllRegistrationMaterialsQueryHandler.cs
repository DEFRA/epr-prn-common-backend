using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Handler for the <see cref="GetAllRegistrationMaterialsQuery"/>.
/// </summary>
/// <param name="registrationMaterialService">Repository for handling materials.</param>
public class GetAllRegistrationMaterialsQueryHandler(
    IRegistrationMaterialRepository registrationMaterialService,
    IMapper mapper
) : IRequestHandler<GetAllRegistrationMaterialsQuery, IList<ApplicantRegistrationMaterialDto>>
{
    /// <inheritdoc />>.
    public async Task<IList<ApplicantRegistrationMaterialDto>> Handle(GetAllRegistrationMaterialsQuery request, CancellationToken cancellationToken)
    {
        // to map.
        var materials = await registrationMaterialService.GetRegistrationMaterialsByRegistrationId(request.RegistrationId);
        return mapper.Map<List<ApplicantRegistrationMaterialDto>>(materials);
    }
}