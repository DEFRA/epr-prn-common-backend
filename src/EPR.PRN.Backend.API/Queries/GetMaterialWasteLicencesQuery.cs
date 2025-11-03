using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetMaterialWasteLicencesQuery : IRequest<RegistrationMaterialWasteLicencesDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}