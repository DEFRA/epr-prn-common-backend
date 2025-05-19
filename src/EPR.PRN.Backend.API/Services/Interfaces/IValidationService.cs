using FluentValidation.Results;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IValidationService
{
    Task<ValidationResult> ValidateAsync<T>(T instance, CancellationToken cancellationToken = default);
    Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default);
}