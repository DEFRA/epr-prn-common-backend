using AutoMapper;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class GetOverseasMaterialReprocessingSitesHandler(
    IMaterialRepository materialRepository,
    IMapper mapper
) : IRequestHandler<GetOverseasMaterialReprocessingSitesQuery, IList<OverseasMaterialReprocessingSiteDto>>
{
    public async Task<IList<OverseasMaterialReprocessingSiteDto>> Handle(GetOverseasMaterialReprocessingSitesQuery request, CancellationToken cancellationToken)
    {
        var sites = await materialRepository.GetOverseasMaterialReprocessingSites(request.RegistrationMaterialId);

        var parentSites = sites
            .Where(site => site.OverseasAddress?.IsInterimSite != true)
            .ToList();

        var result = mapper.Map<IList<OverseasMaterialReprocessingSiteDto>>(parentSites);

        foreach (var dto in result)
        {
            var parent = parentSites
                .Find(x => x.OverseasAddress?.ExternalId == dto.OverseasAddressId);

            if (parent?.OverseasAddress?.ChildInterimConnections == null)
                continue;

            foreach (var conn in parent.OverseasAddress.ChildInterimConnections)
            {
                var interimAddress = conn.OverseasAddress;

                if (interimAddress.IsInterimSite == true)
                {
                    var childDto = mapper.Map<InterimSiteAddressDto>(interimAddress);
                    dto.InterimSiteAddresses?.Add(childDto);
                }
            }
        }

        return result;
    }
}