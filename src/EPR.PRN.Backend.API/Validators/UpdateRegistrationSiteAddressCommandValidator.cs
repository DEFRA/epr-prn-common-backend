using EPR.PRN.Backend.API.Commands;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class UpdateRegistrationSiteAddressCommandValidator : AbstractValidator<UpdateRegistrationSiteAddressCommand>
{
    public UpdateRegistrationSiteAddressCommandValidator()
    {
        RuleFor(x => x.RegistrationId)
            .NotEmpty()
            .WithMessage("RegistrationId is required");

        RuleFor(x => x.ReprocessingSiteAddress)
            .NotNull()
            .WithMessage("Reprocessing Site Address is required");

        When(x => x.ReprocessingSiteAddress?.Id.GetValueOrDefault() == 0, () =>
        {

            RuleFor(x => x.ReprocessingSiteAddress.NationId)
            .NotEmpty()
            .WithMessage("NationId is required");

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
    }
}