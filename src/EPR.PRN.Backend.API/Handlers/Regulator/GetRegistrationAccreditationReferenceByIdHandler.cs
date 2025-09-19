using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetRegistrationAccreditationReferenceByIdHandler(
   IRegistrationMaterialRepository rmRepository) : IRequestHandler<GetRegistrationAccreditationReferenceByIdQuery, RegistrationAccreditationReferenceDto>
{
    public async Task<RegistrationAccreditationReferenceDto> Handle(GetRegistrationAccreditationReferenceByIdQuery request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.Id);

        if (materialEntity?.Registration == null)
        {
            throw new InvalidOperationException("Material entity or its registration is null.");
        }

        var orgType = (ApplicationOrganisationType)materialEntity.Registration.ApplicationTypeId;

        int nationId;
        if (materialEntity.Registration.ApplicationTypeId == 2)
        {
            if (materialEntity.Registration.BusinessAddress?.NationId == null)
            {
                throw new InvalidOperationException("Business address NationId is null.");
            }
            nationId = materialEntity.Registration.BusinessAddress.NationId.Value;
        }
        else
        {
            if (materialEntity.Registration.ReprocessingSiteAddress?.NationId == null)
            {
                throw new InvalidOperationException("Reprocessing site address NationId is null.");
            }
            nationId = materialEntity.Registration.ReprocessingSiteAddress.NationId.Value;
        }

        var referenceRawData = new RegistrationAccreditationReferenceDto()
        {
            ApplicationType = orgType.ToString().First().ToString(),
            OrgCode = materialEntity.RegistrationId.ToString("D6"),
            MaterialCode = materialEntity.Material.MaterialCode,
            NationId = nationId
        };

        return referenceRawData;
    }
}