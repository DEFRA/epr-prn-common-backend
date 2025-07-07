using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers
{
    public class GetMaterialExemptionReferencesHandler(IRegistrationMaterialRepository repository, IMapper mapper) 
        : IRequestHandler<GetMaterialExemptionReferencesQuery, List<GetMaterialExemptionReferenceDto>>
    {
        public async Task<List<GetMaterialExemptionReferenceDto>> Handle(GetMaterialExemptionReferencesQuery request, CancellationToken cancellationToken)
        {
            var response = await repository.GetMaterialExemptionReferences(request.RegistrationMateriaId);

            var model = mapper.Map<List<GetMaterialExemptionReferenceDto>>(response);

            return model;
        }
    }
}

 
