using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class RegistrationOutcomeValidator : AbstractValidator<RegistrationMaterialsOutcomeCommand>
{
    public RegistrationOutcomeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than zero.");


        RuleFor(x => x.RegistrationMaterialStatus)
            .NotEmpty().WithMessage("RegistrationMaterial Status is required.")
            .Must(status => Enum.TryParse(typeof(RegistrationMaterialStatus), status, true, out _))
            .WithMessage("Invalid outcome type.");

        RuleFor(x => x.Comments)
            .MaximumLength(200).WithMessage("RegistrationMaterial Comment cannot exceed 200 characters.");

        //RuleFor(x => x.Comments)
        //.NotEmpty()
        //.When(x => Enum.TryParse(typeof(OutcomeTypes), x.Outcome.ToString(), true, out var outcome) && (OutcomeTypes)outcome == OutcomeTypes.REFUSED)
        //.WithMessage("Comments are required when Outcome is REFUSED.");
    }
}
    

