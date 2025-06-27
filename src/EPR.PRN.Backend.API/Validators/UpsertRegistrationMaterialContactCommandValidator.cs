using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpsertRegistrationMaterialContactCommandValidator : AbstractValidator<UpsertRegistrationMaterialContactCommand>
{
    public UpsertRegistrationMaterialContactCommandValidator()
    {
        RuleFor(x => x.RegistrationMaterialId)
            .NotEmpty()
            .WithMessage("RegistrationMaterialId is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}