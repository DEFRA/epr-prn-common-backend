using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Constants;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class RegistrationOutcomeValidator : AbstractValidator<RegistrationMaterialsOutcomeCommand>
{
    public RegistrationOutcomeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationMessages.RegistrationOutcomeIdRequired)
            .GreaterThan(0).WithMessage(ValidationMessages.RegistrationOutcomeIdGreaterThanZero);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(ValidationMessages.InvalidRegistrationOutcomeStatus);

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage(ValidationMessages.RegistrationOutcomeCommentsMaxLength);

        RuleFor(x => x.Comments)
            .NotEmpty()
            .WithMessage(ValidationMessages.RegistrationOutcomeCommentsCommentsRequired)
            .When(x => x.Status == RegistrationMaterialStatus.Refused);
    }
}