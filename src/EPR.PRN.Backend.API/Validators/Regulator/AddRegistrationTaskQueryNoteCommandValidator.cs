using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Regulator;

public class AddRegistrationTaskQueryNoteCommandValidator : AbstractValidator<AddRegistrationTaskQueryNoteCommand>
{
    public AddRegistrationTaskQueryNoteCommandValidator()
    {
        RuleFor(x => x.RegulatorRegistrationTaskStatusId)
            .NotEmpty().WithMessage("Regulator Registration Task Status Id is required")
            .NotEqual(Guid.Empty).WithMessage("Regulator Registration Task Status is invalid Id ");

        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Note is required")
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("Created by is required ")
            .NotEqual(Guid.Empty).WithMessage("Created by is invalid Id");
    }
}