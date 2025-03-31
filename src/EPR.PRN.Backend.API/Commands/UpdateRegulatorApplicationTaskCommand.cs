using MediatR;
using System.ComponentModel.DataAnnotations;

public class UpdateRegulatorApplicationTaskCommand : UpdateTaskStatusRequestDto, IRequest<bool>
{
    [Required]
    internal int Id { get; set; }
}

