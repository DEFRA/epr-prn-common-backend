using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPR.PRN.Backend.Data
{
    [ExcludeFromCodeCoverage]
    public class EprRegistrationsContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EprRegistrationsContext()
        {

        }

        public EprRegistrationsContext(DbContextOptions<EprRegistrationsContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("EprRegistrationsDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().HasData
            (new Registration { Id=1, ExternalId = "123" });

            modelBuilder.Entity<RegistrationMaterial>().HasData();

            modelBuilder.Entity<RegulatorApplicationTaskStatus>().HasData(
                new Data.RegulatorApplicationTaskStatus { Id = 1 });

            modelBuilder.Entity<RegulatorRegistrationTaskStatus>().HasData();


            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationMaterial> RegistrationMaterials { get; set; }
        public virtual DbSet<RegulatorApplicationTaskStatus> RegulatorApplicationTaskStatus{ get; set; }
        public virtual DbSet<RegulatorRegistrationTaskStatus> RegulatorRegistrationTaskStatus { get; set; }

    }
}