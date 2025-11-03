using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.Data.DTO.Registration;
using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetRegistrationsOverviewByOrgIdQuery : IRequest<IEnumerable<RegistrationOverviewDto>>
    {
        [Required]
        public Guid OrganisationId { get; set; }
    }
}