using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EprPrnIntegration.Common.Models.Rpd;
using FluentValidation;

namespace EPR.PRN.Backend.API.Validators;

public class SavePrnDetailsRequestV2Validator : AbstractValidator<SavePrnDetailsRequestV2>
{
    public SavePrnDetailsRequestV2Validator()
    {
        RuleFor(x => x.SourceSystemId).MandatoryString(PrnConstants.MaxLengthSourceSystemId);

        RuleFor(x => x.PrnNumber).MandatoryString(PrnConstants.MaxLengthPrnNumber);

        RuleFor(x => x.PrnStatusId)
            .MustBeOneOf([(int)EprnStatus.CANCELLED, (int)EprnStatus.AWAITINGACCEPTANCE]);

        RuleFor(x => x.PrnSignatory).MandatoryString(PrnConstants.MaxLengthPrnSignatory);

        RuleFor(x => x.PrnSignatoryPosition)
            .OptionalString(PrnConstants.MaxLengthPrnSignatoryPosition);

        RuleFor(x => x.StatusUpdatedOn).Mandatory();

        RuleFor(x => x.IssuedByOrg).MandatoryString(PrnConstants.MaxLengthIssuedByOrg);

        RuleFor(x => x.OrganisationId).MandatoryGuid();

        RuleFor(x => x.OrganisationName).MandatoryString(PrnConstants.MaxLengthOrganisationName);

        RuleFor(x => x.AccreditationNumber)
            .MandatoryString(PrnConstants.MaxLengthAccreditationNumber);

        RuleFor(x => x.AccreditationYear).MustBeValidYear();

        RuleFor(x => x.MaterialName).MustBeOneOf(RpdMaterialName.GetAll());

        RuleFor(x => x.ReprocessorExporterAgency)
            .MustBeOneOf(RpdReprocessorExporterAgency.GetAll());

        RuleFor(x => x.ReprocessingSite)
            .OptionalString(PrnConstants.MaxLengthReprocessingSite)
            .When(x => x.IsExport == true);

        RuleFor(x => x.ReprocessingSite)
            .MandatoryString(PrnConstants.MaxLengthReprocessingSite)
            .When(x => x.IsExport == false);

        RuleFor(x => x.DecemberWaste).Mandatory();

        RuleFor(x => x.IsExport).Mandatory();

        RuleFor(x => x.TonnageValue)
            .Mandatory()
            .Must(x => x >= 0)
            .WithMessage("{PropertyName} must be valid positive value.");

        RuleFor(x => x.IssuerNotes).OptionalString(PrnConstants.MaxLengthIssuerNotes);

        RuleFor(x => x.ProcessToBeUsed).MustBeOneOf(RpdProcesses.GetAll());

        RuleFor(x => x.ObligationYear).MustBeValidYear();
    }
}

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> MustBeValidYear<T>(
        this IRuleBuilder<T, string?> ruleBuilder
    )
    {
        return ruleBuilder
            .Must(x => int.TryParse(x, out var v) && v is > 1900 and <= 9999)
            .WithMessage("{PropertyName} must be a valid year value.");
    }

    public static IRuleBuilderOptions<T, TProperty?> MustBeOneOf<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        List<TProperty> validValues
    )
    {
        return ruleBuilder
            .Must(x => x != null && validValues.Contains(x))
            .WithMessage($"{{PropertyName}} must be one of {string.Join(", ", validValues)}.");
    }

    public static IRuleBuilderOptions<T, Guid?> MandatoryGuid<T>(
        this IRuleBuilder<T, Guid?> ruleBuilder
    )
    {
        return ruleBuilder
            .Mandatory()
            .Must(x => Guid.TryParse(x.ToString(), out var g) && g != Guid.Empty)
            .WithMessage("{PropertyName} must be a valid GUID");
    }

    public static IRuleBuilderOptions<T, string?> MandatoryString<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int length
    )
    {
        return ruleBuilder
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(length)
            .WithMessage($"{{PropertyName}} cannot be longer than {length} characters.");
    }

    public static IRuleBuilderOptions<T, TProperty?> Mandatory<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder
    )
    {
        return ruleBuilder.NotNull().WithMessage("{PropertyName} is required.");
    }

    public static IRuleBuilderOptions<T, string?> OptionalString<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int length
    )
    {
        return ruleBuilder
            .MaximumLength(length)
            .WithMessage($"{{PropertyName}} cannot be longer than {length} characters.");
    }
}
