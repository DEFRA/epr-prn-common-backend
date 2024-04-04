using AutoMapper;
using EPR.Accreditation.API.Profiles;
using EPR.Accreditation.API.Common.Data;
using Microsoft.EntityFrameworkCore;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Middleware;

namespace EPR.Accreditation.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IHostEnvironment env, IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            services.AddDbContext<AccreditationContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("AccreditationConnnectionString"))
            );

            services.AddDependencies();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AccreditationProfile());
                mc.AddProfile(new EnumProfile());
                mc.AllowNullCollections = true;
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
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
