using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateRegistrationTaskStatusCommandValidator : AbstractValidator<UpdateRegistrationTaskStatusCommandBase>
{
    public UpdateRegistrationTaskStatusCommandValidator()
    {
        var allowedStatuses = new[]
        {
            TaskStatuses.NotStarted,
            TaskStatuses.Started,
            TaskStatuses.CannotStartYet,
            TaskStatuses.Queried, 
            TaskStatuses.Completed 
        };

        RuleFor(x => x.Status)
            .Must(status => allowedStatuses.Contains(status))
            .WithMessage("Invalid Status value");
    }
}