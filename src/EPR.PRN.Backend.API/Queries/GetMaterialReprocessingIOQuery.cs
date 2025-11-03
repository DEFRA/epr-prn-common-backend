using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetMaterialReprocessingIOQuery : IRequest<RegistrationMaterialReprocessingIODto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}