using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DTO;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class RegistrationReprocessingIORequestValidator : AbstractValidator<RegistrationReprocessingIORequestDto>
{
    public RegistrationReprocessingIORequestValidator()
    {
        RuleFor(x => x.TypeOfSuppliers)
            .NotEmpty()
            .WithMessage("Type of suppliers is required")
            .MaximumLength(500)
            .WithMessage("Type of suppliers must not exceed 500 characters");
    }
}