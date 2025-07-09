using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.API.Validators;
using EPR.PRN.Backend.API.Validators.Regulator;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories;
using EPR.PRN.Backend.Data.Repositories.ExporterJourney;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using EPR.PRN.Backend.Data.Repositories.Lookup;
using EPR.PRN.Backend.Data.Repositories.Regulator;
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
                .AddScoped<IValidationService, ValidationService>()
                .AddScoped<IObligationCalculatorService, ObligationCalculatorService>()
                .AddScoped<IMaterialService, MaterialService>()
                .AddScoped<IObligationCalculationRepository, ObligationCalculationRepository>()
                .AddScoped<IRecyclingTargetDataService, RecyclingTargetDataService>()
                .AddScoped<IRecyclingTargetRepository, RecyclingTargetRepository>()
                .AddScoped<IMaterialCalculationStrategyResolver, MaterialCalculationStrategyResolver>()
                .AddScoped<IMaterialCalculationStrategy, GlassCalculationStrategy>()
                .AddScoped<IMaterialCalculationStrategy, GeneralCalculationStrategy>()
                .AddScoped<IMaterialCalculationService, MaterialCalculationService>()
                .AddScoped<IMaterialRepository, MaterialRepository>()
				.AddScoped<IObligationCalculationOrganisationSubmitterTypeRepository, ObligationCalculationOrganisationSubmitterTypeRepository>()
				.AddScoped<IPrnRepository, PrnRepository>()
                .AddScoped<IValidator<SavePrnDetailsRequest>, SavePrnDetailsRequestValidator>()
                .AddScoped<IValidator<UpdateRegistrationCommand>, UpdateRegistrationCommandValidator>()
                .AddScoped<IValidator<UpdateRegulatorRegistrationTaskCommand>, UpdateRegulatorRegistrationTaskCommandValidator>()
                .AddScoped<IValidator<UpdateRegulatorApplicationTaskCommand>, UpdateRegulatorApplicationTaskCommandValidator>()
                .AddScoped<IRegulatorApplicationTaskStatusRepository, RegulatorApplicationTaskStatusRepository>()
                .AddScoped<IRegulatorRegistrationTaskStatusRepository, RegulatorRegistrationTaskStatusRepository>()
                .AddScoped<IValidator<SavePrnDetailsRequest>, SavePrnDetailsRequestValidator>()
                .AddScoped<IRegistrationMaterialRepository, RegistrationMaterialRepository>()
                .AddScoped<IRegulatorAccreditationRepository, RegulatorAccreditationRepository>()
                .AddScoped<IRegulatorAccreditationTaskStatusRepository, RegulatorAccreditationTaskStatusRepository>()
                .AddScoped<IDateTimeProvider, DateTimeProvider>()
                .AddScoped<IAccreditationService, AccreditationService>()
                .AddScoped<IAccreditationPrnIssueAuthService, AccreditationPrnIssueAuthService>()
                .AddScoped<IAccreditationFileUploadService, AccreditationFileUploadService>()
                .AddScoped<IAccreditationRepository, AccreditationRepository>()
                .AddScoped<IAccreditationPrnIssueAuthRepository, AccreditationPrnIssueAuthRepository>()
                .AddScoped<IAccreditationFileUploadRepository, AccreditationFileUploadRepository>()
                .AddScoped<IRegistrationRepository, RegistrationRepository>()
                .AddScoped<ILookupCountryRepository, CountryRepository>()
                .AddScoped<ICarrierBrokerDealerPermitRepository, CarrierBrokerDealerPermitRepository>()
                .AddScoped<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
