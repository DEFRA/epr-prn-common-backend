using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateRegistrationCommandValidator : AbstractValidator<UpdateRegistrationCommand>
{
    public UpdateRegistrationCommandValidator()
    {
        RuleFor(x => x.RegistrationId)
            .NotEmpty()
            .WithMessage("RegistrationId is required");

        RuleFor(x => x.ReprocessingSiteAddress)
            .NotNull()
            .WithMessage("Reprocessing Site Address is required");

        When(x => x.BusinessAddress?.Id.GetValueOrDefault() == 0, () =>
        {

            RuleFor(x => x.BusinessAddress.AddressLine1)
                .NotEmpty()
                .WithMessage("AddressLine1 is required");

            RuleFor(x => x.BusinessAddress.TownCity)
                .NotEmpty()
                .WithMessage("TownCity is required");

            RuleFor(x => x.BusinessAddress.PostCode)
                .NotEmpty()
                .WithMessage("PostCode is required");
        });

        When(x => x.ReprocessingSiteAddress?.Id.GetValueOrDefault() == 0, () =>
        {

            RuleFor(x => x.ReprocessingSiteAddress.GridReference)
                .NotEmpty()
                .WithMessage("GridReference is required");

            RuleFor(x => x.ReprocessingSiteAddress.AddressLine1)
                .NotEmpty()
                .WithMessage("AddressLine1 is required");

            RuleFor(x => x.ReprocessingSiteAddress.TownCity)
                .NotEmpty()
                .WithMessage("TownCity is required");

            RuleFor(x => x.ReprocessingSiteAddress.PostCode)
                .NotEmpty()
                .WithMessage("PostCode is required");
        });

        When(x => x.LegalAddress?.Id.GetValueOrDefault() == 0, () =>
        {

            RuleFor(x => x.LegalAddress.AddressLine1)
                .NotEmpty()
                .WithMessage("AddressLine1 is required");

            RuleFor(x => x.LegalAddress.TownCity)
                .NotEmpty()
                .WithMessage("TownCity is required");

            RuleFor(x => x.LegalAddress.PostCode)
                .NotEmpty()
                .WithMessage("PostCode is required");
        });
    }
}