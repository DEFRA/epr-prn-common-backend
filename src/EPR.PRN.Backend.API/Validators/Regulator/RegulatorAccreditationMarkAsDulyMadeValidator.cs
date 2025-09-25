using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Constants;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Regulator;

public class RegulatorAccreditationMarkAsDulyMadeValidator : AbstractValidator<RegulatorAccreditationMarkAsDulyMadeCommand>
{
    public RegulatorAccreditationMarkAsDulyMadeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationMessages.RegistrationAccreditationIdRequired);

        RuleFor(x => x.DulyMadeDate)
            .Must(date => date != DateTime.MinValue)
            .WithMessage(ValidationMessages.InvalidDulyMadeDate);

        RuleFor(x => x.DeterminationDate)
            .Must(date => date != DateTime.MinValue)
            .WithMessage(ValidationMessages.InvalidDeterminationDate)
            .Must((model, determinationDate) =>
                determinationDate >= model.DulyMadeDate.AddDays(84))
            .WithMessage(ValidationMessages.DeterminationDate12WeekRule);

        RuleFor(x => x.DulyMadeBy)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage(ValidationMessages.DulyMadeByRequired);
    }
}