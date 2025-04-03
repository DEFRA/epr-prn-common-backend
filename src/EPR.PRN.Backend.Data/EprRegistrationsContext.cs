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
        private int numberOfRegistrations = 100;

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
                new LookupRegistrationMaterialStatus { Id = 1, Name = "Grant" },
                new LookupRegistrationMaterialStatus { Id = 2, Name = "Refuse" });

            modelBuilder.Entity<LookupTaskStatus>().HasData(
                new LookupTaskStatus { Id = 1, Name = "NotStarted" },
                new LookupTaskStatus { Id = 2, Name = "Started" },
                new LookupTaskStatus { Id = 3, Name = "CanNotStartYet" },
                new LookupTaskStatus { Id = 4, Name = "Queried" },
                new LookupTaskStatus { Id = 5, Name = "Completed" });

            modelBuilder.Entity<LookupApplicationType>().HasData(
                new LookupApplicationType { Id = 1, Name = "Reprocessor" },
                new LookupApplicationType { Id = 2, Name = "Exporter" });

            modelBuilder.Entity<LookupTask>().HasData(
                new LookupTask { Id = 1, Name = "SiteAddressAndContactDetails" },
                new LookupTask { Id = 2, Name = "WasteLicensesPermitsAndExemptions" },
                new LookupTask { Id = 3, Name = "ReprocessingInputsAndOutputs" },
                new LookupTask { Id = 4, Name = "SamplingAndInspectionPlan" },
                new LookupTask { Id = 5, Name = "RegistrationDulyMade" },
                new LookupTask { Id = 6, Name = "AssignOfficer" },
                new LookupTask { Id = 7, Name = "MaterialsAuthorisedOnSite" },
                new LookupTask { Id = 8, Name = "MaterialDetailsAndContact" },
                new LookupTask { Id = 9, Name = "OverseasReprocessorAndInterimSiteDetails" },
                new LookupTask { Id = 10, Name = "BusinessAddress" });



            var registrations = new List<Registration>();
            var lookupAddresses = new List<LookupAddress>();
            var registrationMaterials = new List<RegistrationMaterial>();
            var registrationTaskStatuses = new List<RegulatorRegistrationTaskStatus>();
            var applicationTaskStatuses = new List<RegulatorApplicationTaskStatus>();


            var RegulatorApplicationTaskStatuscounter = 1;
            var RegulatorRegistrationTaskStatuscounter = 1;
            for (int registrationcounter = 1; registrationcounter <= numberOfRegistrations; registrationcounter++)
            {
                var ApplicationTypeId = registrationcounter % 2 + 1;
                registrations.Add(new Registration
                {
                    Id = registrationcounter,
                    ExternalId = Guid.NewGuid().ToString(),
                    ApplicationTypeId = ApplicationTypeId,
                    OrganisationId = 1,
                    //RegistrationStatusId = registrationcounter % 5 + 1,
                    BusinessAddressId = registrationcounter,
                    ReprocessingSiteAddressId = 2
                });

                lookupAddresses.Add(new LookupAddress
                {
                    Id = registrationcounter,
                    AddressLine1 = "23",
                    AddressLine2 = "Ruby St",
                    TownCity = "London",
                    County = "Liverpool",
                    Country = "England",
                    PostCode = "E12 3SE"
                });

                if (ApplicationTypeId == 1)
                {
                    RegulatorRegistrationTaskStatuscounter = AddReprocessorRegistrationTasks(registrationTaskStatuses, RegulatorRegistrationTaskStatuscounter, registrationcounter);
                }
                else
                {
                    RegulatorRegistrationTaskStatuscounter = AddExporterRegistrationTasks(registrationTaskStatuses, RegulatorRegistrationTaskStatuscounter, registrationcounter);

                }

                for (int j = 1; j <= 3; j++)
                {
                    var registrationMaterialId = (registrationcounter - 1) * 3 + j;
                    registrationMaterials.Add(new RegistrationMaterial
                    {
                        Id = registrationMaterialId,
                        MaterialId = j,
                        //StatusID = 1,
                        RegistrationId = registrationcounter,
                        DeterminationDate = DateTime.UtcNow,
                        ReferenceNumber = $"REF{registrationcounter:D4}-{j:D2}",
                        Comments = $"Test description for material {j} in registration {registrationcounter}"
                    });

                    if (ApplicationTypeId == 1)
                    {
                        RegulatorApplicationTaskStatuscounter = AddReprocessorRegistrationMaterialTasks(applicationTaskStatuses, RegulatorApplicationTaskStatuscounter, registrationMaterialId);
                    }
                    else
                    {
                        RegulatorApplicationTaskStatuscounter = AddExporterRegistrationMaterialTasks(applicationTaskStatuses, RegulatorApplicationTaskStatuscounter, registrationMaterialId);

                    }
                }
            }

            modelBuilder.Entity<Registration>().HasData(registrations);
            modelBuilder.Entity<LookupAddress>().HasData(lookupAddresses);
            modelBuilder.Entity<RegistrationMaterial>().HasData(registrationMaterials);
            modelBuilder.Entity<RegulatorRegistrationTaskStatus>().HasData(registrationTaskStatuses);
            modelBuilder.Entity<RegulatorApplicationTaskStatus>().HasData(applicationTaskStatuses);

            base.OnModelCreating(modelBuilder);
        }

        private static int AddReprocessorRegistrationMaterialTasks(List<RegulatorApplicationTaskStatus> applicationTaskStatuses, int RegulatorApplicationTaskStatuscounter, int registrationMaterialId)
        {
            applicationTaskStatuses.Add(new RegulatorApplicationTaskStatus
            {
                Id = RegulatorApplicationTaskStatuscounter,
                TaskId = 2,
                RegistrationMaterialId = registrationMaterialId,
                TaskStatusId = 1
            });
            RegulatorApplicationTaskStatuscounter++;
            applicationTaskStatuses.Add(new RegulatorApplicationTaskStatus
            {
                Id = RegulatorApplicationTaskStatuscounter,
                TaskId = 3,
                RegistrationMaterialId = registrationMaterialId,
                TaskStatusId = 1
            });
            RegulatorApplicationTaskStatuscounter++;
            applicationTaskStatuses.Add(new RegulatorApplicationTaskStatus
            {
                Id = RegulatorApplicationTaskStatuscounter,
                TaskId = 4,
                RegistrationMaterialId = registrationMaterialId,
                TaskStatusId = 1
            });
            RegulatorApplicationTaskStatuscounter++;
            return RegulatorApplicationTaskStatuscounter;
        }

        private static int AddExporterRegistrationMaterialTasks(List<RegulatorApplicationTaskStatus> applicationTaskStatuses, int RegulatorApplicationTaskStatuscounter, int registrationMaterialId)
        {
            applicationTaskStatuses.Add(new RegulatorApplicationTaskStatus
            {
                Id = RegulatorApplicationTaskStatuscounter,
                TaskId = 8,
                RegistrationMaterialId = registrationMaterialId,
                TaskStatusId = 1
            });
            RegulatorApplicationTaskStatuscounter++;
            applicationTaskStatuses.Add(new RegulatorApplicationTaskStatus
            {
                Id = RegulatorApplicationTaskStatuscounter,
                TaskId = 9,
                RegistrationMaterialId = registrationMaterialId,
                TaskStatusId = 1
            });
            RegulatorApplicationTaskStatuscounter++;
            applicationTaskStatuses.Add(new RegulatorApplicationTaskStatus
            {
                Id = RegulatorApplicationTaskStatuscounter,
                TaskId = 4,
                RegistrationMaterialId = registrationMaterialId,
                TaskStatusId = 1
            });
            RegulatorApplicationTaskStatuscounter++;
            return RegulatorApplicationTaskStatuscounter;
        }

        private static int AddReprocessorRegistrationTasks(List<RegulatorRegistrationTaskStatus> registrationTaskStatuses, int RegulatorRegistrationTaskStatuscounter, int registrationcounter)
        {
            registrationTaskStatuses.Add(new RegulatorRegistrationTaskStatus
            {
                Id = RegulatorRegistrationTaskStatuscounter,
                TaskId = 1,
                RegistrationId = registrationcounter,
                TaskStatusId = 1
            });
            RegulatorRegistrationTaskStatuscounter++;
            registrationTaskStatuses.Add(new RegulatorRegistrationTaskStatus
            {
                Id = RegulatorRegistrationTaskStatuscounter,
                TaskId = 7,
                RegistrationId = registrationcounter,
                TaskStatusId = 1
            });
            RegulatorRegistrationTaskStatuscounter++;
            return RegulatorRegistrationTaskStatuscounter;
        }
        private static int AddExporterRegistrationTasks(List<RegulatorRegistrationTaskStatus> registrationTaskStatuses, int RegulatorRegistrationTaskStatuscounter, int registrationcounter)
        {
            registrationTaskStatuses.Add(new RegulatorRegistrationTaskStatus
            {
                Id = RegulatorRegistrationTaskStatuscounter,
                TaskId = 10,
                RegistrationId = registrationcounter,
                TaskStatusId = 1
            });
            RegulatorRegistrationTaskStatuscounter++;
            registrationTaskStatuses.Add(new RegulatorRegistrationTaskStatus
            {
                Id = RegulatorRegistrationTaskStatuscounter,
                TaskId = 2,
                RegistrationId = registrationcounter,
                TaskStatusId = 1
            });
            RegulatorRegistrationTaskStatuscounter++;
            return RegulatorRegistrationTaskStatuscounter;
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