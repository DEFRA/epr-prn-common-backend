namespace EPR.PRN.Backend.API.Validators
{
    using EPR.PRN.Backend.API.Common.DTO;
    using FluentValidation;

    public class SavePrnDetailsRequestValidator : AbstractValidator<SavePrnDetailsRequest>
    {
        public SavePrnDetailsRequestValidator()
        {
            RuleFor(x => x.AccreditationNo)
                .NotEmpty().WithMessage("AccreditationNo is required.")
                .MaximumLength(20).WithMessage("AccreditionNo cannot be longer than 20 characters");

            RuleFor(x => x.AccreditationYear)
                .NotEmpty().WithMessage("AccreditationYear is required.")
                .Must(x => x > 1900 && x <= 9999).WithMessage("AccreditationYear must be a valid year value");

            RuleFor(x => x.ObligationYear)
                .NotEmpty().WithMessage("ObligationYear is required.")
                .Must(x => x > 1900 && x <= 9999).WithMessage("ObligationYear must be a valid year value");

            RuleFor(x => x.DecemberWaste)
                .NotNull().WithMessage("DecemberWaste is required.");

            RuleFor(x => x.EvidenceMaterial)
                .NotEmpty().WithMessage("EvidenceMaterial is required.")
                .MaximumLength(20).WithMessage("EvidenceMaterial cannot be longer than 20 characters");

            RuleFor(x => x.EvidenceNo)
                .NotEmpty().WithMessage("EvidenceNo is required.")
                .MaximumLength(20).WithMessage("EvidenceNo cannot be longer than 20 characters");

            RuleFor(x => x.EvidenceStatusCode)
                .NotNull().WithMessage("EvidenceStatusCode is required.");

            RuleFor(x => x.EvidenceTonnes)
                .NotNull().WithMessage("EvidenceTonnes is required.");

            RuleFor(x => x.IssueDate)
                .NotNull().WithMessage("IssueDate is required.");

            RuleFor(x => x.IssuedByOrgName)
                .NotEmpty().WithMessage("IssuedByOrgName is required.")
                .MaximumLength(50).WithMessage("IssuedByOrgName cannot be longer than 50 characters");

            RuleFor(x => x.IssuedToOrgName)
                .NotEmpty().WithMessage("IssuedToOrgName is required.")
                .MaximumLength(50).WithMessage("IssuedToOrgName cannot be longer than 50 characters");

            RuleFor(x => x.IssuedToEPRId)
                .NotNull().WithMessage("IssuedToEPRId is required.")
                .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("Invalid IssuedToEPRId. It must be a valid Guid.");

            RuleFor(x => x.ExternalId)
                .NotNull().WithMessage("ExternalId is required.")
                .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("Invalid ExternalId. It must be a valid Guid.");

            RuleFor(x => x.ProducerAgency)
                .NotEmpty().WithMessage("ProducerAgency is required.")
                .MaximumLength(100).WithMessage("ProducerAgency cannot be longer than 100 characters");

            RuleFor(x => x.RecoveryProcessCode)
                .NotEmpty().WithMessage("RecoveryProcessCode is required.")
                .MaximumLength(20).WithMessage("RecoveryProcessCode cannot be longer than 20 characters");

            RuleFor(x => x.ReprocessorAgency)
                .NotEmpty().WithMessage("ReprocessorAgency is required.")
                .MaximumLength(100).WithMessage("ReprocessorAgency cannot be longer than 100 characters");

            RuleFor(x => x.StatusDate)
                .NotNull().WithMessage("StatusDate is required.");

            RuleFor(x => x.CancelledDate).NotNull()
                .When(x => x.EvidenceStatusCode == Data.DataModels.EprnStatus.CANCELLED)
                .WithMessage("CancelledDate is required when the request has cancelled status");

            RuleFor(x => x.IssuerNotes)
                 .MaximumLength(200).WithMessage("IssuerNotes cannot be longer than 200 characters")
                 .When(x => !string.IsNullOrWhiteSpace(x.IssuerNotes));

            RuleFor(x => x.IssuerRef)
                 .NotNull().WithMessage("IssuerRef is required.")
                 .MaximumLength(200).WithMessage("IssuerNotes cannot be longer than 200 characters");

            RuleFor(x => x.PrnSignatory)
                .MaximumLength(50).WithMessage("PrnSignatory cannot be longer than 50 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.PrnSignatory));

            RuleFor(x => x.PrnSignatoryPosition)
                .MaximumLength(50).WithMessage("PrnSignatoryPosition cannot be longer than 50 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.PrnSignatoryPosition));
        }
    }
}
