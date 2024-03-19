using EPR.Accreditation.API.Repositories;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services;
using EPR.Accreditation.API.Services.Interfaces;

namespace EPR.Accreditation.API.Helpers
{
    public static class DependencyHelper
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services
                .AddScoped<IRepository, Repository>()
                .AddScoped<IAccreditationService, AccreditationService>()
                .AddScoped<ICountryService, CountryService>();

            return services;
        }
    }
}
