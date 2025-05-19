using EPR.PRN.Backend.API.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace EPR.PRN.Backend.API.Services;


public class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<ValidationResult> ValidateAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>() ?? throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
        return await validator.ValidateAsync(instance, cancellationToken);
    }

    public async Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>() ?? throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
        await validator.ValidateAndThrowAsync(instance, cancellationToken);
    }

}