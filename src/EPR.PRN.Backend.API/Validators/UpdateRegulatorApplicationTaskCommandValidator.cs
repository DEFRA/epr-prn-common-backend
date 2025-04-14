using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;
public class UpdateRegulatorApplicationTaskCommandValidator : AbstractValidator<UpdateRegulatorApplicationTaskCommand>
{
    public UpdateRegulatorApplicationTaskCommandValidator()
    {
        var allowedStatuses = new[] { StatusTypes.Queried, StatusTypes.Completed};

        RuleFor(x => x.Status)
            .Must(status => allowedStatuses.Contains(status))
            .WithMessage("Invalid Status value");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters");

        RuleFor(x => x.Comment)
            .NotEmpty().When(x => x.Status == StatusTypes.Queried)
            .WithMessage("Comment is required when status is Queried");

    }
}