using FluentValidation;

public class UpdateTaskStatusRequestDtoValidator : AbstractValidator<UpdateTaskStatusRequestDto>
{
    public UpdateTaskStatusRequestDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid Status value");

        RuleFor(x => x.Comment)
            .MaximumLength(200).WithMessage("Comment must not exceed 200 characters");
    }
}