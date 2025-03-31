using MediatR;
using System.ComponentModel.DataAnnotations;

public class UpdateRegulatorApplicationTaskCommand : RegistrationTaskStatusDto, IRequest<bool>
{
    [Required]
    internal int Id { get; set; }
}

