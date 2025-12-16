using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class SavePrnDetailsRequestV2Validator : AbstractValidator<SavePrnDetailsRequestV2>
{
    public SavePrnDetailsRequestV2Validator()
    {
        RuleFor(x => x.AccreditationNumber)
            .NotEmpty().WithMessage("AccreditationNumber is required.")
            .MaximumLength(PrnConstants.MaxLengthAccreditationNumber)
            .WithMessage(
                $"AccreditationNumber cannot be longer than {PrnConstants.MaxLengthAccreditationNumber} characters");

        RuleFor(x => x.AccreditationYear)
            .NotEmpty().WithMessage("AccreditationYear is required.")
            .Must(x => int.TryParse(x, out var v) && v is > 1900 and <= 9999)
            .WithMessage("AccreditationYear must be a valid year value");

        RuleFor(x => x.DecemberWaste)
            .NotNull().WithMessage("DecemberWaste is required.");

        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("MaterialName is required.")
            .MaximumLength(PrnConstants.MaxLengthMaterialName)
            .WithMessage($"MaterialName cannot be longer than {PrnConstants.MaxLengthMaterialName} characters");

        RuleFor(x => x.PrnNumber)
            .NotEmpty().WithMessage("PrnNumber is required.")
            .MaximumLength(PrnConstants.MaxLengthPrnNumber)
            .WithMessage($"PrnNumber cannot be longer than {PrnConstants.MaxLengthPrnNumber} characters");

        RuleFor(x => x.PrnStatusId)
            .NotNull().WithMessage("PrnStatusId is required.");

        RuleFor(x => x.TonnageValue)
            .NotNull().WithMessage("TonnageValue is required.")
            .Must(x => x >= 0).WithMessage("TonnageValue must be greater than 0");

        RuleFor(x => x.IssueDate)
            .NotNull().WithMessage("IssueDate is required.");

        RuleFor(x => x.IssuedByOrg)
            .NotEmpty().WithMessage("IssuedByOrg is required.")
            .MaximumLength(PrnConstants.MaxLengthIssuedByOrg)
            .WithMessage($"IssuedByOrg cannot be longer than {PrnConstants.MaxLengthIssuedByOrg} characters");

        RuleFor(x => x.OrganisationName)
            .NotEmpty().WithMessage("OrganisationName is required.")
            .MaximumLength(PrnConstants.MaxLengthOrganisationName).WithMessage(
                $"OrganisationName cannot be longer than {PrnConstants.MaxLengthOrganisationName} characters");

        RuleFor(x => x.OrganisationId)
            .NotEmpty().WithMessage("OrganisationId is required.")
            .Must(x => Guid.TryParse(x.ToString(), out var g) && g != Guid.Empty)
            .WithMessage("Invalid OrganisationId. It must be a valid Guid.");

        RuleFor(x => x.ProducerAgency)
            .NotEmpty().WithMessage("ProducerAgency is required.")
            .MaximumLength(PrnConstants.MaxLengthProducerAgency)
            .WithMessage($"ProducerAgency cannot be longer than {PrnConstants.MaxLengthProducerAgency} characters");

        RuleFor(x => x.ProcessToBeUsed)
            .NotEmpty().WithMessage("ProcessToBeUsed is required.")
            .MaximumLength(PrnConstants.MaxLengthProcessToBeUsed).WithMessage(
                $"RecoveryProcessCode cannot be longer than {PrnConstants.MaxLengthProcessToBeUsed} characters");

        RuleFor(x => x.ReprocessorExporterAgency)
            .NotEmpty().WithMessage("ReprocessorExporterAgency is required.")
            .MaximumLength(PrnConstants.MaxLengthReprocessorExporterAgency).WithMessage(
                $"ReprocessorExporterAgency cannot be longer than {PrnConstants.MaxLengthReprocessorExporterAgency} characters");

        RuleFor(x => x.StatusUpdatedOn)
            .NotNull().WithMessage("StatusUpdatedOn is required.");

        RuleFor(x => x.IssuerNotes)
            .MaximumLength(PrnConstants.MaxLengthIssuerNotes)
            .WithMessage($"IssuerNotes cannot be longer than {PrnConstants.MaxLengthIssuerNotes} characters")
            .When(x => !string.IsNullOrWhiteSpace(x.IssuerNotes));

        RuleFor(x => x.IssuerReference)
            .MaximumLength(PrnConstants.MaxLengthIssuerReference)
            .WithMessage($"IssuerRef cannot be longer than {PrnConstants.MaxLengthIssuerReference} characters")
            .When(x => !string.IsNullOrWhiteSpace(x.IssuerReference));

        RuleFor(x => x.PrnSignatory)
            .MaximumLength(PrnConstants.MaxLengthPrnSignatory)
            .WithMessage($"PrnSignatory cannot be longer than {PrnConstants.MaxLengthPrnSignatory} characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PrnSignatory));

        RuleFor(x => x.PrnSignatoryPosition)
            .MaximumLength(PrnConstants.MaxLengthPrnSignatoryPosition).WithMessage(
                $"PrnSignatoryPosition cannot be longer than {PrnConstants.MaxLengthPrnSignatoryPosition} characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PrnSignatoryPosition));

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedByUser is required.")
            .MaximumLength(PrnConstants.MaxLengthCreatedBy)
            .WithMessage("CreatedByUser cannot be longer than 20 characters");

        RuleFor(x => x.SourceSystemId)
            .NotEmpty().WithMessage("SourceSystemId is required.")
            .MaximumLength(PrnConstants.MaxLengthSourceSystemId).WithMessage(
                $"SourceSystemId cannot be longer than {PrnConstants.MaxLengthSourceSystemId} characters");
        
        RuleFor(x => x.PackagingProducer)
            .NotEmpty().WithMessage("PackagingProducer is required.")
            .MaximumLength(PrnConstants.MaxLengthPackagingProducer).WithMessage(
                $"PackagingProducer cannot be longer than {PrnConstants.MaxLengthPackagingProducer} characters");
      
        RuleFor(x => x.ReprocessingSite)
            .NotEmpty().WithMessage("ReprocessingSite is required.")
            .MaximumLength(PrnConstants.MaxLengthReprocessingSite).WithMessage(
                $"ReprocessingSite cannot be longer than {PrnConstants.MaxLengthReprocessingSite} characters");
       
        RuleFor(x => x.Signature)
            .MaximumLength(PrnConstants.MaxLengthSignature).WithMessage(
                $"Signature cannot be longer than {PrnConstants.MaxLengthSignature} characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Signature));
    }
}