using MediatR;
using System.ComponentModel.DataAnnotations;

public class UpdateRegulatorRegistrationTaskCommand: UpdateTaskStatusRequestDto, IRequest<bool>
{
    [Required]
    internal int Id { get; set; }
}

