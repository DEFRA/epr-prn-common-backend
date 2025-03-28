﻿using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.API.Validators;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Repositories;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Providers;
using EPR.PRN.Backend.Obligation.Services;
using EPR.PRN.Backend.Obligation.Strategies;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DependencyHelper
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>()
                .AddScoped<IPrnService, PrnService>()
                .AddScoped<IObligationCalculatorService, ObligationCalculatorService>()
                .AddScoped<IObligationCalculationRepository, ObligationCalculationRepository>()
                .AddScoped<IRecyclingTargetDataService, RecyclingTargetDataService>()
                .AddScoped<IRecyclingTargetRepository, RecyclingTargetRepository>()
                .AddScoped<IMaterialCalculationStrategyResolver, MaterialCalculationStrategyResolver>()
                .AddScoped<IMaterialCalculationStrategy, GlassCalculationStrategy>()
                .AddScoped<IMaterialCalculationStrategy, GeneralCalculationStrategy>()
                .AddScoped<IMaterialCalculationService, MaterialCalculationService>()
                .AddScoped<IMaterialRepository, MaterialRepository>()
                .AddScoped<IPrnRepository, PrnRepository>()
                .AddScoped<IValidator<SavePrnDetailsRequest>, SavePrnDetailsRequestValidator>()
                .AddScoped<IDateTimeProvider, DateTimeProvider>();

			return services;
        }
    }
}
