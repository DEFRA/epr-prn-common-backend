using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator
{
    public class GetAccreditationSamplingPlanQueryHandler(
        IRegistrationMaterialRepository rmRepository,
        IMapper mapper
    ) : IRequestHandler<GetAccreditationSamplingPlanQuery, AccreditationSamplingPlanDto>
    {
        public async Task<AccreditationSamplingPlanDto> Handle(GetAccreditationSamplingPlanQuery request, CancellationToken cancellationToken)
        {
            var accreditation = await rmRepository.GetAccreditation_FileUploadById(request.Id);
            var accreditationDto = mapper.Map<AccreditationSamplingPlanDto>(accreditation);
            return accreditationDto;
        }
    }
}
