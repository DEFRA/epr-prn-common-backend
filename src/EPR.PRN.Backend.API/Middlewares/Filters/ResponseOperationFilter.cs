using EPR.PRN.Backend.API.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EPR.PRN.Backend.API.Middlewares.Filters
{
    public class ResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodType = context?.ApiDescription.HttpMethod;
            var controllerType = context?.MethodInfo.DeclaringType;
            var methodname = context?.MethodInfo.Name;

            if(controllerType == typeof(RegistrationMaterialController) && methodType == "POST")
            {

            }
        }
    }
}
