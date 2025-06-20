using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

    public class GetRegistrationByIdQuery : IRequest<RegistrationOverviewDto>
    {
        [Required]
        public Guid Id { get; set; }
    }

