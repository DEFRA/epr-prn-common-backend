using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using EPR.PRN.Backend.API.Validators.Regulator;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Middlewares;
using EPR.PRN.Backend.Data;

using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation;

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
            services.AddFeatureManagement();
            services.AddLogging();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
            services.AddApiVersioning();
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            });
            services
             .AddFluentValidationAutoValidation(options =>
             { 
             options.DisableDataAnnotationsValidation = true; // if you don't want DataAnnotations
             })
             .AddFluentValidationClientsideAdapters()
             .AddValidatorsFromAssemblyContaining<RegistrationOutcomeValidator>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(s => s.FullName);
                config.DocumentFilter<FeatureEnabledDocumentFilter>();
                config.OperationFilter<FeatureGateOperationFilter>();

                // This is the key part:
                config.TagActionsBy(api =>
                {
                    var tag = api.ActionDescriptor.EndpointMetadata
                        .OfType<SwaggerOperationAttribute>()
                        .Select(attr => attr.Tags?.FirstOrDefault())
                        .FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(tag))
                        return new[] { tag };
                    return new[] { api.ActionDescriptor.RouteValues["controller"] };
                });
            });
            services.AddFeatureManagement();

			services.AddDbContext<EprContext>(options =>
				options.UseSqlServer(_config.GetConnectionString("EprConnectionString"), sqlOptions =>
				{
					sqlOptions.CommandTimeout(300);
				}));

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
                    app.UseExceptionHandler(env.IsDevelopment() ? "/error-development" : "/error");
                    app.UseMiddleware<ExceptionHandlingMiddleware>();
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