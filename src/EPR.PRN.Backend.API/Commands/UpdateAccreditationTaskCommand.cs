using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.API.Common.Enums;
using MediatR;

namespace EPR.PRN.Backend.API.Commands
{
    public class UpdateAccreditationTaskCommand : IRequest
    {

        [Required]
        public required Guid AccreditationId { get; set; }

        [Required]
        [MaxLength(200)]
        public required string TaskName { get; set; }

        [Required]
       // [MaxLength(50)]
        public required TaskStatuses Status { get; set; }


    }
}
