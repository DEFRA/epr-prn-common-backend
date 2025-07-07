using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetMaterialExemptionReferencesQuery : IRequest<List<GetMaterialExemptionReferenceDto>>
    {
        public Guid RegistrationMateriaId { get; set; }
    }
}
