using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetMaterialWasteLicencesQuery : IRequest<RegistrationMaterialWasteLicencesDto>
    {
        [Required]
        public int Id { get; set; }
    }
}