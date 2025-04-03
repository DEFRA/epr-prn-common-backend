
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;
using Polly;
namespace EPR.PRN.Backend.API.Validators;

public class RegistrationOutcomeValidator : AbstractValidator<RegistrationMaterialsOutcomeCommand>
{
    public RegistrationOutcomeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than zero.");


        RuleFor(x => x.Outcome)
            .NotEmpty().WithMessage("Outcome is required.")
            .Must(outcome => Enum.TryParse(typeof(OutcomeStatus), outcome, true, out _))
            .WithMessage("Invalid outcome type.");

        RuleFor(x => x.Comments)
            .MaximumLength(200).WithMessage("OutcomeComment cannot exceed 200 characters.");

        //RuleFor(x => x.Comments)
        //.NotEmpty()
        //.When(x => Enum.TryParse(typeof(OutcomeTypes), x.Outcome.ToString(), true, out var outcome) && (OutcomeTypes)outcome == OutcomeTypes.REFUSED)
        //.WithMessage("Comments are required when Outcome is REFUSED.");
    }
}
    

