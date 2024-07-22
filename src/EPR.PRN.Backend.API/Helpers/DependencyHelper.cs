using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.API.Services.Interfaces;

namespace EPR.PRN.Backend.API.Helpers
{
    public static class DependencyHelper
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services
                .AddScoped<IRepository, Repository>()
                .AddScoped<IPrnService, PrnService>();

            return services;
        }
    }
}
