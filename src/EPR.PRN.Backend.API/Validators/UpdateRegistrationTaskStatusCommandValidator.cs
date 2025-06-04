using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateRegistrationTaskStatusCommandValidator : AbstractValidator<UpdateRegistrationTaskStatusCommand>
{
    public UpdateRegistrationTaskStatusCommandValidator()
    {
        var allowedStatuses = new[] 
        { 
            TaskStatuses.Started, 
            TaskStatuses.Queried, 
            TaskStatuses.Completed 
        };

        RuleFor(x => x.Status)
            .Must(status => allowedStatuses.Contains(status))
            .WithMessage("Invalid Status value");
    }
}