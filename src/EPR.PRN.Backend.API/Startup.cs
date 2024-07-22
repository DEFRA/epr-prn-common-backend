using EPR.PRN.Backend.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.API.Services.Interfaces;

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
			services.AddControllers();
			//services
			//		.AddScoped<IRepository, Repository>()
			//		.AddScoped<IPrnService, PrnService>();
            services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(config =>
			{
				config.CustomSchemaIds(s => s.FullName);
			});
			services.AddDbContext<EprContext>(options =>
				options.UseSqlServer(_config.GetConnectionString("EprnConnectionString"))
			);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

