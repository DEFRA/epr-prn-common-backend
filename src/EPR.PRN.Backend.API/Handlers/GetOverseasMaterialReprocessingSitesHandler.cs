using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
    
/// <summary>
/// Handler for the <see cref="GetAllRegistrationMaterialsQuery"/>.
/// </summary>
/// <param name="registrationMaterialService">Repository for handling materials.</param>
public class GetOverseasMaterialReprocessingSitesHandler(
    IRegistrationMaterialRepository registrationMaterialService,
    IMapper mapper
) : IRequestHandler<GetOverseasMaterialReprocessingSitesQuery, IList<OverseasMaterialReprocessingSiteDto>>
{
    /// <inheritdoc />>.
    public async Task<IList<OverseasMaterialReprocessingSiteDto>> Handle(GetOverseasMaterialReprocessingSitesQuery request, CancellationToken cancellationToken)
    {
        var overseasMaterialReprocessingSites = await registrationMaterialService.GetOverseasMaterialReprocessingSites(request.RegistrationMaterialId);

        //todo create map profile for overseasMaterialReprocessingSites to OverseasMaterialReprocessingSiteDto and return the mapped list.
        return mapper.Map<IList<OverseasMaterialReprocessingSiteDto>>(overseasMaterialReprocessingSites);
    }
}
