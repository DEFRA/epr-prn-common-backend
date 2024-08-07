﻿using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data;
using Microsoft.EntityFrameworkCore;
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Temporarily disable to troubleshoot deployment issues
            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

