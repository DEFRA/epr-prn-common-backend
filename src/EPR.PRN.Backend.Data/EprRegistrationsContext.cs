using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

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
            modelBuilder.Entity<LookupMaterial>().HasData(
                    new LookupMaterial { Id = 1, Name = "Plastic", MaterialCode = "PL" },
                    new LookupMaterial { Id = 2, Name = "Steel", MaterialCode = "GL" });
            modelBuilder.Entity<LookupRegistrationMaterialStatus>().HasData(
                    new LookupRegistrationMaterialStatus { Id = 1, Name = "Approved" },
                    new LookupRegistrationMaterialStatus { Id = 2, Name = "Not Started" });
            modelBuilder.Entity<LookupTaskStatus>().HasData(
                  new LookupTaskStatus { Id = 1, Name = "GRANTED" },
                  new LookupTaskStatus { Id = 2, Name = "REFUSED" });


            modelBuilder.Entity<Registration>().HasData
            (new Registration { Id = 1, ExternalId = "123" });

            modelBuilder.Entity<RegistrationMaterial>().HasData();

            modelBuilder.Entity<RegulatorApplicationTaskStatus>().HasData(
                new RegulatorApplicationTaskStatus { Id = 1 });

            modelBuilder.Entity<RegulatorRegistrationTaskStatus>().HasData();


            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationMaterial> RegistrationMaterials { get; set; }
        public virtual DbSet<RegulatorApplicationTaskStatus> RegulatorApplicationTaskStatus { get; set; }
        public virtual DbSet<RegulatorRegistrationTaskStatus> RegulatorRegistrationTaskStatus { get; set; }
        public DbSet<LookupMaterial> LookupMaterials { get; set; }
        public DbSet<LookupRegistrationMaterialStatus> LookupRegistrationMaterialStatuses { get; set; }
        public DbSet<LookupTask> LookupTasks { get; set; }
        public DbSet<LookupRegistrationStatus> LookupRegistrationStatuses { get; set; }
        public DbSet<LookupTaskStatus> LookupTaskStatuses { get; set; }

    }
}