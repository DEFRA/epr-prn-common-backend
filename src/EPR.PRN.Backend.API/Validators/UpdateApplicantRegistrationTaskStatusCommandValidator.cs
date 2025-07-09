using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateApplicantRegistrationTaskStatusCommandValidator : AbstractValidator<UpdateApplicantRegistrationTaskStatusCommand>
{
    public UpdateApplicantRegistrationTaskStatusCommandValidator()
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