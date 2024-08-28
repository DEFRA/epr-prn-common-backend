using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(s => s.FullName);
            });

            services.AddDbContext<EprContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("EprConnectionString"))
            );

            services.AddDependencies();

            AddHealthChecks(services);
        }

        private void AddHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
            .AddSqlServer(
                _config.GetConnectionString("EprConnectionString")!,
                failureStatus: HealthStatus.Unhealthy,
                tags: ["Database"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                RunMigration(app);
            }

            //app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Temporarily disable to troubleshoot deployment issues
            // app.UseHttpsRedirection();
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

