using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data;

[ExcludeFromCodeCoverage]
public class EprRegistrationsContext : DbContext
{
    private const int NumberOfRegistrations = 100;

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
            new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            new LookupRegistrationMaterialStatus { Id = 2, Name = "Refused" });

        modelBuilder.Entity<LookupTaskStatus>().HasData(
            new LookupTaskStatus { Id = 1, Name = "NotStarted" },
            new LookupTaskStatus { Id = 2, Name = "Started" },
            new LookupTaskStatus { Id = 3, Name = "CannotStartYet" },
            new LookupTaskStatus { Id = 4, Name = "Queried" },
            new LookupTaskStatus { Id = 5, Name = "Completed" });

        modelBuilder.Entity<LookupApplicationType>().HasData(
            new LookupApplicationType { Id = 1, Name = "Reprocessor" },
            new LookupApplicationType { Id = 2, Name = "Exporter" });

        modelBuilder.Entity<LookupJourneyType>().HasData(
            new LookupJourneyType { Id = 1, Name = "Registration" },
            new LookupJourneyType { Id = 2, Name = "Accreditation" });

        modelBuilder.Entity<LookupRegulatorTask>().HasData(

            new LookupRegulatorTask { Id = 1, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "SiteAddressAndContactDetails" },
            new LookupRegulatorTask { Id = 2, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "MaterialsAuthorisedOnSite" },
            new LookupRegulatorTask { Id = 3, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "RegistrationDulyMade" },
            new LookupRegulatorTask { Id = 4, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "WasteLicensesPermitsAndExemptions" },
            new LookupRegulatorTask { Id = 5, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "ReprocessingInputsAndOutputs" },
            new LookupRegulatorTask { Id = 6, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "SamplingAndInspectionPlan" },
            new LookupRegulatorTask { Id = 7, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = "AssignOfficer" },
            new LookupRegulatorTask { Id = 8, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "BusinessAddress" },
            new LookupRegulatorTask { Id = 9, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "WasteLicensesPermitsAndExemptions" },
            new LookupRegulatorTask { Id = 10, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "RegistrationDulyMade" },
            new LookupRegulatorTask { Id = 11, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "SamplingAndInspectionPlan" },
            new LookupRegulatorTask { Id = 12, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "AssignOfficer" },
            new LookupRegulatorTask { Id = 13, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "MaterialDetailsAndContact" },
            new LookupRegulatorTask { Id = 14, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = "OverseasReprocessorAndInterimSiteDetails" });

        var registrations = new List<Registration>();
        var lookupAddresses = new List<LookupAddress>();
        var registrationMaterials = new List<RegistrationMaterial>();
        var registrationTaskStatuses = new List<RegulatorRegistrationTaskStatus>();
        var applicationTaskStatuses = new List<RegulatorApplicationTaskStatus>();

        var addressTemplates = new[]
        {
            ("23", "Ruby St", "London", "England", "E12 3SE", 1),
            ("78", "Pine Ln", "Belfast", "Northern Ireland", "BT1 3FG", 2),
            ("45", "Maple Ave", "Edinburgh", "Scotland", "EH3 5DN", 3),
            ("12", "Oak Rd", "Cardiff", "Wales", "CF10 1AA", 4)
        };

        LookupAddress CreateAddressTemplate((string line1, string line2, string city, string country, string postCode, int nationId) tpl, int id)
        {
            return new LookupAddress
            {
                Id = id,
                AddressLine1 = tpl.line1,
                AddressLine2 = tpl.line2,
                TownCity = tpl.city,
                County = null,
                Country = tpl.country,
                PostCode = tpl.postCode,
                NationId = tpl.nationId,
                GridReference = $"SJ 854 66{id}"
            };
        }

        for (var registrationCounter = 1; registrationCounter <= NumberOfRegistrations; registrationCounter++)
        {
            var ApplicationTypeId = registrationCounter % 2 + 1;

            registrations.Add(new Registration
            {
                Id = registrationCounter,
                ExternalId = Guid.NewGuid().ToString(),
                ApplicationTypeId = ApplicationTypeId,
                OrganisationId = 1,
                BusinessAddressId = ApplicationTypeId == 2 ? registrationCounter : null,
                ReprocessingSiteAddressId = ApplicationTypeId == 1 ? registrationCounter : null,
                LegalDocumentAddressId = registrationCounter
            });

            var template = addressTemplates[Random.Shared.Next(addressTemplates.Length)];
            lookupAddresses.Add(CreateAddressTemplate(template, registrationCounter));

            var hasRegisteredMaterial = false;
            var materials = new List<RegistrationMaterial>();

            for (int j = 1; j <= 3; j++)
            {
                bool isLast = j == 3;
                bool isRegistered = isLast && !hasRegisteredMaterial || Random.Shared.Next(2) == 1;

                hasRegisteredMaterial |= isRegistered;

                materials.Add(new RegistrationMaterial
                {
                    Id = (registrationCounter - 1) * 3 + j,
                    MaterialId = j,
                    StatusID = null,
                    RegistrationId = registrationCounter,
                    DeterminationDate = DateTime.UtcNow,
                    ReferenceNumber = $"REF{registrationCounter:D4}-{j:D2}",
                    Comments = $"Test description for material {j} in registration {registrationCounter}",
                    ReasonforNotreg = isRegistered
                        ? string.Empty
                        : $"Lorem ipsum dolor sit amet, consectetur adipiscing{j} elit. Fusce vulputate aliquet ornare. Vestibulum dolor nunc, tincidunt a diam nec, mattis venenatis sem{registrationCounter}",
                    Wastecarrierbrokerdealerregistration = $"DFG3457345{registrationCounter}",
                    IsMaterialRegistered = isRegistered
                });
            }

            registrationMaterials.AddRange(materials);
        }

        modelBuilder.Entity<Registration>().HasData(registrations);
        modelBuilder.Entity<LookupAddress>().HasData(lookupAddresses);
        modelBuilder.Entity<RegistrationMaterial>().HasData(registrationMaterials);
        modelBuilder.Entity<RegulatorRegistrationTaskStatus>().HasData(registrationTaskStatuses);
        modelBuilder.Entity<RegulatorApplicationTaskStatus>().HasData(applicationTaskStatuses);

        modelBuilder.Entity<Registration>()
            .HasMany(r => r.Tasks);

        modelBuilder.Entity<Registration>()
            .HasMany(r => r.Materials);

        modelBuilder.Entity<RegistrationMaterial>()
            .HasMany(r => r.Tasks)
            .WithOne()
            .HasForeignKey(t => t.RegistrationMaterialId);

        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Registration> Registrations { get; set; }
    public virtual DbSet<RegistrationMaterial> RegistrationMaterials { get; set; }
    public virtual DbSet<RegulatorApplicationTaskStatus> RegulatorApplicationTaskStatus { get; set; }
    public virtual DbSet<RegulatorRegistrationTaskStatus> RegulatorRegistrationTaskStatus { get; set; }
    public DbSet<LookupMaterial> LookupMaterials { get; set; }
    public DbSet<LookupRegistrationMaterialStatus> LookupRegistrationMaterialStatuses { get; set; }
    public DbSet<LookupRegulatorTask> LookupTasks { get; set; }
    public DbSet<LookupRegistrationStatus> LookupRegistrationStatuses { get; set; }
    public DbSet<LookupTaskStatus> LookupTaskStatuses { get; set; }
    public DbSet<LookupAddress> LookupAddresses { get; set; }
}