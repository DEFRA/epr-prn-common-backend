using EPR.PRN.Backend.API.Commands.ExporterJourney;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Validators.ExportJourney
{
    [ExcludeFromCodeCoverage(Justification = "Will be implemented upon successful testing")]
    public class UpsertAddressForServiceOfNoticesCommandValidator : AbstractValidator<UpsertAddressForServiceOfNoticesCommand>
    {
        public UpsertAddressForServiceOfNoticesCommandValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage("LegalDocumentAddress is required");

            RuleFor(x => x.Dto.LegalDocumentAddress)
                .NotNull().WithMessage("LegalDocumentAddress is required");

            When(x => x.Dto.LegalDocumentAddress.Id.GetValueOrDefault() == 0, () => {

                RuleFor(x => x.Dto.LegalDocumentAddress.AddressLine1)
                    .NotEmpty()
                    .WithMessage("AddressLine1 is required");

                RuleFor(x => x.Dto.LegalDocumentAddress.TownCity)
                    .NotEmpty()
                    .WithMessage("TownCity is required");

                RuleFor(x => x.Dto.LegalDocumentAddress.PostCode)
                    .NotEmpty()
                    .WithMessage("PostCode is required");
            });
        }
	}
}
