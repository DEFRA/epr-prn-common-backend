using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateRegistrationMaterialPermitCapacityCommandValidator : AbstractValidator<UpdateRegistrationMaterialPermitCapacityCommand>
{
    public UpdateRegistrationMaterialPermitCapacityCommandValidator()
    {
        RuleFor(x => x.PermitTypeId)
            .NotEmpty()
            .WithMessage("PermitTypeId is required");

        RuleFor(x => x.CapacityInTonnes)
            .NotNull()
            .WithMessage("Weight must be a number greater than 0");

        RuleFor(x => x.CapacityInTonnes)
            .Must(x => x != 0)
            .WithMessage("Weight must be a number greater than 0");

        RuleFor(x => x.CapacityInTonnes)
            .Must(x => x.GetValueOrDefault() < 10000000)
            .WithMessage("Weight must be a number less than 10,000,000")
            .When(x => x.CapacityInTonnes.GetValueOrDefault() > 0);
    }
}
