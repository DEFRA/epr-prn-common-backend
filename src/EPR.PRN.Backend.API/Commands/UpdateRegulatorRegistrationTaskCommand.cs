using MediatR;
using System.ComponentModel.DataAnnotations;

public class UpdateRegulatorRegistrationTaskCommand: RegistrationTaskStatusDto, IRequest<bool>
{
    [Required]
    internal int Id { get; set; }
}

