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
            .IsInEnum()
            .WithMessage("Invalid registration material status.");

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage("RegistrationMaterial Comment cannot exceed 200 characters.");

        RuleFor(x => x.Comments)
            .NotEmpty()
            .WithMessage("Comments are required.")
            .When(x => x.RegistrationMaterialStatus == RegistrationMaterialStatus.Refused);
    }
}