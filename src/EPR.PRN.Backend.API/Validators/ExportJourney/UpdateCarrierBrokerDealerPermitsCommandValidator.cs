using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators.ExportJourney
{
    public class UpdateCarrierBrokerDealerPermitsCommandValidator : AbstractValidator<UpdateCarrierBrokerDealerPermitsCommand>
    {
        public UpdateCarrierBrokerDealerPermitsCommandValidator()
        {
            var allowedStatuses = new[] { RegulatorTaskStatus.Queried, RegulatorTaskStatus.Completed };

            RuleFor(x => x.Dto.WasteCarrierBrokerDealerRegistration)
                .MaximumLength(16)
                .WithMessage("WasteCarrierBrokerDealerRegistration must not exceed 16 characters");

            RuleFor(x => x.Dto.WasteLicenseOrPermitNumber)
                .MaximumLength(20)
                .WithMessage("WasteLicenseOrPermitNumber must not exceed 20 characters");

            RuleFor(x => x.Dto.PpcNumber)
                .MaximumLength(20)
                .WithMessage("PpcNumber must not exceed 20 characters");

            RuleFor(x => x.Dto.WasteExemptionReference)
                .NotNull()
                .WithMessage("WasteExemptionReference is required");

            RuleFor(x => x.Dto.WasteExemptionReference)
                .Must(list => list == null || list.Count <= 5)
                .WithMessage("WasteExemptionReference must not exceed 5 values")
                .When(x => x.Dto.WasteExemptionReference != null);

            RuleForEach(x => x.Dto.WasteExemptionReference)
                .MaximumLength(20)
                .WithMessage("WasteExemptionReference must not exceed 20 characters")
                .When(x => x.Dto.WasteExemptionReference != null);
        }
    }
}
