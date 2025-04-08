using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Validators;
using EPR.PRN.Backend.Data;

using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;

namespace EPR.PRN.Backend.API
{
    [ExcludeFromCodeCoverage]
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IHostEnvironment env, IConfiguration config)
		{
			_config = config;
		}

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegistrationMaterialsOutcomeHandler>());
            services.AddApiVersioning();
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            });
            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<RegistrationOutcomeValidator>();
                fv.AutomaticValidationEnabled = false;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(s => s.FullName);
                config.DocumentFilter<FeatureEnabledDocumentFilter>();
                config.OperationFilter<FeatureGateOperationFilter>();
            });
            services.AddFeatureManagement();

            services.AddDbContext<EprContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("EprConnectionString"))
            );
            if (_config.GetValue<bool>($"FeatureManagement:{FeatureFlags.ReprocessorExporter}"))
            {
                services.AddDbContext<EprRegistrationsContext>(options =>
                    options.UseInMemoryDatabase("EprRegistrationsDatabase")
                );
            }
            else
            {
                services.AddDbContext<EprRegistrationsContext>();
            }
            
            services.AddDbContext<EprRegistrationsContext>(options =>
                options.UseInMemoryDatabase("EprRegistrationsDatabase")
            );

            services.AddDependencies();

            services.Configure<PrnObligationCalculationConfig>(_config.GetSection(PrnObligationCalculationConfig.SectionName));

            AddHealthChecks(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var featureManager = scope.ServiceProvider.GetRequiredService<IFeatureManager>();
                if (featureManager.IsEnabledAsync(FeatureFlags.ReprocessorExporter).Result)
                {
                    var context = scope.ServiceProvider.GetRequiredService<EprRegistrationsContext>();
                    context.Database.EnsureCreated();
                }
            }
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                RunMigration(app);
            }
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/admin/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }).AllowAnonymous();
            }); 
        }

        private void AddHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
            .AddSqlServer(
                _config.GetConnectionString("EprConnectionString")!,
                failureStatus: HealthStatus.Unhealthy,
                tags: ["Database"]);
        }

        private void RunMigration(IApplicationBuilder app)
        {
            if (_config.GetValue<bool>("RunMigration"))
            {
                using var scope = app.ApplicationServices.CreateScope();

                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<EprContext>>();
                var context = services.GetService<EprContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(EprContext).Name);

                    context?.Database.Migrate();

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(EprContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(EprContext).Name);
                }
            }
        }
    }
}