using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    new LookupMaterial { Id = 1, MaterialName = "Plastic", MaterialCode = "PL" },
                    new LookupMaterial { Id = 2, MaterialName = "Steel", MaterialCode = "GL" },
                    new LookupMaterial { Id = 3, MaterialName = "Aluminium", MaterialCode = "AL" });

            modelBuilder.Entity<LookupRegistrationMaterialStatus>().HasData(
                    new LookupRegistrationMaterialStatus { Id = 1, Name = "Approved" },
                    new LookupRegistrationMaterialStatus { Id = 2, Name = "Not Started" },
                    new LookupRegistrationMaterialStatus { Id = 3, Name = "Completed" });

            modelBuilder.Entity<LookupRegistrationStatus>().HasData(
             new LookupRegistrationStatus { Id = 1, Name = "Not Started" },
            new LookupRegistrationStatus { Id = 2, Name = "Started" },
            new LookupRegistrationStatus { Id = 3, Name = "Can Not Start Yet" },
            new LookupRegistrationStatus { Id = 4, Name = "Queried"  },
            new LookupRegistrationStatus { Id = 5, Name = "Completed" });
            

            modelBuilder.Entity<LookupTaskStatus>().HasData(
                  new LookupTaskStatus { Id = 1, Name = "GRANTED" },
                  new LookupTaskStatus { Id = 2, Name = "REFUSED" });

            modelBuilder.Entity<LookupApplicationType>().HasData(
                  new LookupApplicationType { Id = 1, Name = "Reprocessor" },
                  new LookupApplicationType { Id = 2, Name = "Exporter" });
            modelBuilder.Entity<LookupTask>().HasData(
             new LookupTask { Id = 1, Name = "Waste licences, permits or exemptions" },
            new LookupTask { Id = 2, Name = "Reprocessing inputs and outputs" },
            new LookupTask { Id = 3, Name = "Sampling and inspection plan" });

            modelBuilder.Entity<LookupAddress>().HasData(
             new LookupAddress{ Id = 1, AddressLine1 = "23", AddressLine2 = "Ruby St",TownCity = "London",County="Liverpool" ,Country = "EngLAnd", PostCode = "E12 3SE"},
            new LookupAddress { Id = 2, AddressLine1 = "27", AddressLine2 = "Ruby Street",TownCity = "London", County = "Liverpool", Country = "EngLAnd", PostCode = "NN1 1NN"});
             
        modelBuilder.Entity<Registration>().HasData(
            new Registration { Id = 1, ExternalId = "123",ApplicationTypeId=1, OrganisationId=1, RegistrationStatusId=1 , BusinessAddressId =1 },
            new Registration { Id = 2, ExternalId = "123", ApplicationTypeId = 2, OrganisationId = 1, RegistrationStatusId = 2, ReprocessingSiteAddressId = 2 });


        modelBuilder.Entity<RegistrationMaterial>().HasData(
                new RegistrationMaterial
                {
                    Id = 101,
                    MaterialId = 1,
                    StatusID = 1,
                    RegistrationId=1,
                    DeterminationDate = DateTime.UtcNow,
                    ReferenceNumber = "DEF4567",
                    Comments = "Test description for Plastic"
                },
                new RegistrationMaterial { Id = 102, MaterialId = 2,
                    RegistrationId = 1, StatusID = 2, DeterminationDate = DateTime.UtcNow,
                    ReferenceNumber = "DEF456", Comments = "Test description for Steel" },
                new RegistrationMaterial
                {
                    Id = 103,
                    MaterialId = 3,
                    RegistrationId = 2,
                    StatusID = 2,
                    DeterminationDate = DateTime.UtcNow,
                    ReferenceNumber = "DEF4569",
                    Comments = "Test description for Aluminium"
                });
            modelBuilder.Entity<RegistrationTaskStatus>().HasData(
            new RegistrationTaskStatus
            {
                Id = 1, 
                TaskId = 1,
                RegistrationId = 1,
                StatusId =1
            });
            modelBuilder.Entity<RegistrationTaskStatus>().HasData(
            new RegistrationTaskStatus
            {
                Id = 2, 
                TaskId = 2,
                RegistrationId = 2,
                StatusId = 2
            });
            modelBuilder.Entity<RegistrationTaskStatus>().HasData(
            new RegistrationTaskStatus
            {
                Id = 3, 
                TaskId = 3,
                RegistrationId = 1,
                StatusId = 3
            });           
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
        public virtual DbSet<RegistrationTaskStatus> RegistrationTaskStatus { get; set; }

    }
}