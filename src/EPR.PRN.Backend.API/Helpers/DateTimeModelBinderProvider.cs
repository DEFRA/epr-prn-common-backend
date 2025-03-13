using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EPR.PRN.Backend.API.Helpers;

public class DateTimeModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
        {
            return new DateTimeModelBinder("yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fff");
        }

        return null;
    }
}