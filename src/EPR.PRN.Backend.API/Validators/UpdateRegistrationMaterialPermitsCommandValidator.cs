using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateRegistrationMaterialPermitsCommandValidator : AbstractValidator<UpdateRegistrationMaterialPermitsCommand>
{
    public UpdateRegistrationMaterialPermitsCommandValidator()
    {
        RuleFor(x => x.PermitTypeId)
            .NotEmpty()
            .WithMessage("PermitTypeId is required");

        RuleFor(x => x.PermitNumber)
            .NotEmpty()
            .WithMessage("PermitNumber is required");
    }
}
