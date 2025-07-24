using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateMaterialNotReprocessingReasonCommandValidator : AbstractValidator<UpdateMaterialNotReprocessingReasonCommand>
{
    public UpdateMaterialNotReprocessingReasonCommandValidator()
    {
        RuleFor(x => x.MaterialNotReprocessingReason)
            .NotEmpty()
            .WithMessage("Material not reprocessing reason is required")
            .MaximumLength(500)
            .WithMessage("Material not reprocessing reason must not exceed 500 characters");
    }
}