using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;
public class UpdateRegulatorRegistrationTaskCommandValidator : AbstractValidator<UpdateRegulatorRegistrationTaskCommand>
{
    public UpdateRegulatorRegistrationTaskCommandValidator()
    {
        var allowedStatuses = new[] { RegulatorTaskStatus.Queried, RegulatorTaskStatus.Completed };

        RuleFor(x => x.Status)
            .Must(status => allowedStatuses.Contains(status))
            .WithMessage("Invalid Status value");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters");

        RuleFor(x => x.Comment)
            .NotEmpty().When(x => x.Status == RegulatorTaskStatus.Queried)
            .WithMessage("Comment is required when status is Queried");

    }
}