using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.Config;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Services;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DependencyHelper
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            var settings = services.BuildServiceProvider().GetRequiredService<IOptions<CommonDataApiConfig>>().Value;

            services
                .AddScoped<IRepository, Repository>()
                .AddScoped<IPrnService, PrnService>()
                .AddScoped<IObligationCalculatorService, ObligationCalculatorService>()
                .AddScoped<IRecyclingTargetDataService, RecyclingTargetDataService>();


            services.AddHttpClient<IPomSubmissionData, PomSubmissionData>((sp, client) =>
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.Timeout);
            })
                .AddPolicyHandler(GetRetryPolicy(settings.ServiceRetryCount));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
