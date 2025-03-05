using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EPR.PRN.Backend.API.Helpers
{
    [ExcludeFromCodeCoverage]
    public class DateTimeModelBinder : IModelBinder
    {
        private readonly string[] _dateFormats;
        
        public DateTimeModelBinder(params string[] dateFormats)
        {
            _dateFormats = dateFormats;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }
            
            foreach (var format in _dateFormats)
            {
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    bindingContext.Result = ModelBindingResult.Success(date);
                    return Task.CompletedTask;
                }
            }

            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName,
                $"The date format should be one of the following: {string.Join(", ", _dateFormats)}.");

            return Task.CompletedTask;
        }
    }
}
