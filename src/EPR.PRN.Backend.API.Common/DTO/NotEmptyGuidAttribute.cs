using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Common.Dto;

public sealed class NotEmptyGuidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is Guid guid && guid == Guid.Empty)
        {
            return new ValidationResult(
                $"{context.MemberName} must not be an empty GUID.",
                new[] { context.MemberName! });
        }

        return ValidationResult.Success;
    }
}