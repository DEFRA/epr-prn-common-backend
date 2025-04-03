using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

public class UpdateRegulatorRegistrationTaskCommandValidator : AbstractValidator<UpdateRegulatorRegistrationTaskCommand>
{
    public UpdateRegulatorRegistrationTaskCommandValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid Status value");

        RuleFor(x => x.Comment)
            .MaximumLength(200).WithMessage("Comment must not exceed 200 characters");

        RuleFor(x => x.Comment)
            .NotEmpty().When(x => x.Status == StatusTypes.Queried)
            .WithMessage("Comment is required when status is Queried");

    }
}