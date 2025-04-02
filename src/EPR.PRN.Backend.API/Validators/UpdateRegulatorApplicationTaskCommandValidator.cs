using FluentValidation;

public class UpdateRegulatorApplicationTaskCommandValidator : AbstractValidator<UpdateRegulatorApplicationTaskCommand>
{
    public UpdateRegulatorApplicationTaskCommandValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid Status value");

        RuleFor(x => x.Comment)
            .MaximumLength(200).WithMessage("Comment must not exceed 200 characters");
    }
}