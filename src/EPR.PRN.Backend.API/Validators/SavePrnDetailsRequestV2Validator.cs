using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class SavePrnDetailsRequestV2Validator : AbstractValidator<SavePrnDetailsRequestV2>
{
    public SavePrnDetailsRequestV2Validator()
    {
        RuleFor(x => x.SourceSystemId).MandatoryString(PrnConstants.MaxLengthSourceSystemId);
        
        RuleFor(x => x.PrnNumber).OptionalString(PrnConstants.MaxLengthPrnNumber);

        RuleFor(x => x.PrnSignatory).MandatoryString(PrnConstants.MaxLengthPrnSignatory);

        RuleFor(x => x.PrnSignatoryPosition).OptionalString(PrnConstants.MaxLengthPrnSignatoryPosition);

        RuleFor(x => x.IssuedByOrg).MandatoryString(PrnConstants.MaxLengthIssuedByOrg);
        
        RuleFor(x => x.OrganisationId).MandatoryGuid();

        RuleFor(x => x.OrganisationName).MandatoryString(PrnConstants.MaxLengthOrganisationName);

        RuleFor(x => x.AccreditationNumber).MandatoryString(PrnConstants.MaxLengthAccreditationNumber);

        RuleFor(x => x.AccreditationYear)
            .MandatoryString(PrnConstants.MaxLengthAccreditationYear)
            .Must(x => int.TryParse(x, out var v) && v is > 1900 and <= 9999)
            .WithMessage("{PropertyName} must be a valid year value.");

        RuleFor(x => x.MaterialName).MandatoryString(PrnConstants.MaxLengthMaterialName);

        RuleFor(x => x.ReprocessingSite).MandatoryString(PrnConstants.MaxLengthReprocessingSite);

        RuleFor(x => x.TonnageValue)
            .Must(x => x >= 0).WithMessage("{PropertyName} must be valid positive value.");

        RuleFor(x => x.IssuerNotes).OptionalString(PrnConstants.MaxLengthIssuerNotes);

        RuleFor(x => x.ReprocessorExporterAgency).OptionalString(PrnConstants.MaxLengthReprocessorExporterAgency);
    }
}

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> MandatoryGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder.Must(x => Guid.TryParse(x.ToString(), out var g) && g != Guid.Empty)
            .WithMessage("{PropertyName} must be a valid GUID");
        
    }
    public static IRuleBuilderOptions<T, string> MandatoryString<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(length).WithMessage($"{{PropertyName}} cannot be longer than {length} characters.");
    }
    public static IRuleBuilderOptions<T, string?> OptionalString<T>(this IRuleBuilder<T, string?> ruleBuilder, int length)
    {
        return ruleBuilder
            .MaximumLength(length).WithMessage($"{{PropertyName}} cannot be longer than {length} characters.");
    }
}            