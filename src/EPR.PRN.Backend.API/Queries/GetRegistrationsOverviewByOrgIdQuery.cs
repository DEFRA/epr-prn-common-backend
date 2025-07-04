using EPR.PRN.Backend.Data.DTO.Registration;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Queries
{
    public class GetRegistrationsOverviewByOrgIdQuery : IRequest<IEnumerable<RegistrationOverviewDto>>
    {
        [Required]
        public Guid OrganisationId { get; set; }
    }
}