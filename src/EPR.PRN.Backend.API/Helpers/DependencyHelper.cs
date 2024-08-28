using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Repositories;
using EPR.PRN.Backend.Obligation.Config;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Services;
using EPR.PRN.Backend.Obligation.Strategies;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace EPR.PRN.Backend.API.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DependencyHelper
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            var settings = services.BuildServiceProvider().GetRequiredService<IOptions<CommonDataApiConfig>>().Value;

            services.AddScoped<IRepository, Repository>()
                .AddScoped<IPrnService, PrnService>()
                .AddScoped<IObligationCalculatorService, ObligationCalculatorService>()
                .AddScoped<IObligationCalculationRepository, ObligationCalculationRepository>()
                .AddScoped<IRecyclingTargetDataService, RecyclingTargetDataService>()
                .AddScoped<IRecyclingTargetRepository, RecyclingTargetRepository>()
                .AddScoped<IMaterialCalculationStrategyResolver, MaterialCalculationStrategyResolver>()
                .AddScoped<IMaterialCalculationStrategy, GlassCalculationStrategy>()
                .AddScoped<IMaterialCalculationStrategy, GeneralCalculationStrategy>()
                .AddScoped<IMaterialCalculationService, MaterialCalculationService>();

            services.AddHttpClient<IPomSubmissionData, PomSubmissionData>((sp, client) =>
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.Timeout);
            })
                .AddPolicyHandler(GetRetryPolicy(settings.ServiceRetryCount));

            return services;
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
