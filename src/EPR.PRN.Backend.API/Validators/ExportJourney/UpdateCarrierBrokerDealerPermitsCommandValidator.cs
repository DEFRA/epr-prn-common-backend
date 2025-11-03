using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Commands.ExporterJourney;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.ExportJourney
{
    [ExcludeFromCodeCoverage(Justification = "Will be implemented upon successful testing")]
    public class UpdateCarrierBrokerDealerPermitsCommandValidator : AbstractValidator<UpdateCarrierBrokerDealerPermitsCommand>
    {
        public UpdateCarrierBrokerDealerPermitsCommandValidator()
        {
            RuleFor(x => x.Dto.WasteCarrierBrokerDealerRegistration)
                .MaximumLength(16).WithMessage("WasteCarrierBrokerDealerRegistration must not exceed 16 characters");

            RuleFor(x => x.Dto.WasteLicenseOrPermitNumber)
                .MaximumLength(20).WithMessage("WasteLicenseOrPermitNumber must not exceed 20 characters");

            RuleFor(x => x.Dto.PpcNumber)
                .MaximumLength(20).WithMessage("PpcNumber must not exceed 20 characters");

            When(x => x.Dto.WasteExemptionReference != null, () =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                RuleFor(x => x.Dto.WasteExemptionReference)
                .Must(item => item.Count <= 5).WithMessage("WasteExemptionReference must not exceed 5 values")
                .ForEach(item =>
                    item.MaximumLength(20).WithMessage("WasteExemptionReference must not exceed 20 characters")
                );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
        }
    }
}
