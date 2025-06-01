using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.Regulator;

public class AddApplicationTaskQueryNoteCommandValidator : AbstractValidator<AddApplicationTaskQueryNoteCommand>
{
    public AddApplicationTaskQueryNoteCommandValidator()
    {
        RuleFor(x => x.RegulatorApplicationTaskStatusId)
            .NotEmpty().WithMessage("Regulator Application Task Status Id is required")
            .NotEqual(Guid.Empty).WithMessage("Regulator Application Task Status is invalid Id ");           

        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Note is required")
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters");

        RuleFor(x => x.QueryBy)
            .NotEmpty().WithMessage("Query by is required ")
            .NotEqual(Guid.Empty).WithMessage("Query by is invalid Id");
    }
}