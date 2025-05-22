using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Constants;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class MarkAsDulyMadeValidator : AbstractValidator<RegistrationMaterialsMarkAsDulyMadeCommand>
{
    public MarkAsDulyMadeValidator()
    {
        RuleFor(x => x.RegistrationMaterialId)
            .NotEmpty().WithMessage(ValidationMessages.RegistrationMaterialIdRequired);

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