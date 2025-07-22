using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DTO.Accreditiation;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator
{
    public class GetAccreditationsOverviewByOrgIdQueryHandler(
        IAccreditationRepository accreditationRepository,
        IValidationService validationService
        )
        : IRequestHandler<GetAccreditationsOverviewByOrgIdQuery, IEnumerable<AccreditationOverviewDto>>
    {
        public async Task<IEnumerable<AccreditationOverviewDto>> Handle(GetAccreditationsOverviewByOrgIdQuery request, CancellationToken cancellationToken)
        {
            await validationService.ValidateAndThrowAsync(request, cancellationToken);
            return await accreditationRepository.GetAccreditationOverviewForOrgId(request.OrganisationId);
        }
    }
}
