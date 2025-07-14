using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateMaterialNotRegisteringReasonCommandValidator : AbstractValidator<UpdateMaterialNotRegisteringReasonCommand>
{
    public UpdateMaterialNotRegisteringReasonCommandValidator()
    {
        RuleFor(x => x.MaterialNotRegisteringReason)
            .NotEmpty()
            .WithMessage("Material not registering reason is required")
            .MaximumLength(500)
            .WithMessage("Material not registering reason must not exceed 500 characters");
    }
}