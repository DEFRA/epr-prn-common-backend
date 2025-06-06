using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetAccreditationSamplingPlanQuery : IRequest<AccreditationSamplingPlanDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
