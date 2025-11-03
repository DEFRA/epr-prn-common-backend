using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.Data.DTO.Accreditiation;
using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetAccreditationsOverviewByOrgIdQuery : IRequest<IEnumerable<AccreditationOverviewDto>>
    {
        [Required]
        public Guid OrganisationId { get; set; }
    }
}
