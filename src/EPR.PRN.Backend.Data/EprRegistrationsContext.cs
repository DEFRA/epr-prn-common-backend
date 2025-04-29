using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

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

    int registrationId;
    int lookupAddressID;
    int registrationMaterialId;
    int materialExemptionReferenceId;
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

        modelBuilder.Entity<LookupMaterialPermit>().HasData(
            new LookupMaterialPermit { Id = 1, Name = PermitTypes.WasteExemption },
            new LookupMaterialPermit { Id = 2, Name = PermitTypes.PollutionPreventionAndControlPermit },
            new LookupMaterialPermit { Id = 3, Name = PermitTypes.WasteManagementLicence },
            new LookupMaterialPermit { Id = 4, Name = PermitTypes.InstallationPermit },
            new LookupMaterialPermit { Id = 5, Name = PermitTypes.EnvironmentalPermitOrWasteManagementLicence });

        modelBuilder.Entity<LookupPeriod>().HasData(
           new LookupPeriod { Id = 1, Name = "Per Year" },
           new LookupPeriod { Id = 2, Name = "Per Month" },
           new LookupPeriod { Id = 3, Name = "Per Week" });

        modelBuilder.Entity<LookupFileUploadType>().HasData(
            new LookupFileUploadType { Id = 1, Name = "SamplingAndInspectionPlan" });

        modelBuilder.Entity<LookupFileUploadStatus>().HasData(
            new LookupFileUploadStatus { Id = 1, Name = "Virus check failed" },
            new LookupFileUploadStatus { Id = 2, Name = "Virus check succeeded" },
            new LookupFileUploadStatus { Id = 3, Name = "Upload complete" },
            new LookupFileUploadStatus { Id = 4, Name = "Upload failed" },
            new LookupFileUploadStatus { Id = 5, Name = "File deleted(Soft delete of record in database – will physically remove from blob storage)" });

        var registrations = new List<Registration>();
        var lookupAddresses = new List<LookupAddress>();
        var registrationMaterials = new List<RegistrationMaterial>();
        var materialExemptionReferences = new List<MaterialExemptionReference>();
        var registrationTaskStatuses = new List<RegulatorRegistrationTaskStatus>();
        var applicationTaskStatuses = new List<RegulatorApplicationTaskStatus>();
        var registrationReprocessingIOs = new List<RegistrationReprocessingIO>();
        var fileUploads = new List<FileUpload>();

        registrationId = 0;
        lookupAddressID = 0;
        registrationMaterialId = 0;
        materialExemptionReferenceId = 0;

        for (var registrationCounter = 1; registrationCounter <= NumberOfRegistrations; registrationCounter++)
        {
            var ApplicationTypeId = registrationCounter % 2 + 1;

            lookupAddresses.Add(new LookupAddress
            {
                Id = ++lookupAddressID,
                AddressLine1 = "23",
                AddressLine2 = "Ruby St",
                TownCity = "London",
                County = null,
                Country = "England",
                PostCode = "E12 3SE"
            });

            registrations.Add(new Registration
            {
                Id = ++registrationId,
                ExternalId = Guid.NewGuid().ToString(),
                ApplicationTypeId = ApplicationTypeId,
                OrganisationId = 1,
                BusinessAddressId = lookupAddressID,
                ReprocessingSiteAddressId = lookupAddressID
            });

            for (int j = 1; j <= 3; j++)
            {
                registrationMaterials.Add(GetRegistrationMaterial(registrationCounter, registrationId, j, materialExemptionReferences));

                registrationReprocessingIOs.Add(GetReprocessionIos(registrationCounter, registrationMaterialId));

                fileUploads.AddRange(GetFileUploads(registrationCounter, j, registrationMaterialId));
            }
        }


        modelBuilder.Entity<Registration>().HasData(registrations);
        modelBuilder.Entity<LookupAddress>().HasData(lookupAddresses);
        modelBuilder.Entity<RegistrationMaterial>().HasData(registrationMaterials);
        modelBuilder.Entity<MaterialExemptionReference>().HasData(materialExemptionReferences);
        modelBuilder.Entity<RegistrationReprocessingIO>().HasData(registrationReprocessingIOs);
        modelBuilder.Entity<FileUpload>().HasData(fileUploads);
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

    private RegistrationMaterial GetRegistrationMaterial(int registrationCounter, int registrationId, int j, List<MaterialExemptionReference> materialExemptionReferences)
    {
        int permitTypeId = 1;
        if (registrationCounter <= 10)
        {
            permitTypeId = 1;
        }
        else if (registrationCounter > 10 && registrationCounter <= 20)
        {
            permitTypeId = 1;
        }
        else if (registrationCounter > 20 && registrationCounter <= 30)
        {
            permitTypeId = 1;
        }
        else if (registrationCounter > 30 && registrationCounter <= 40)
        {
            permitTypeId = 2;
        }
        else if (registrationCounter > 40 && registrationCounter <= 50)
        {
            permitTypeId = 3;
        }
        else if (registrationCounter > 50 && registrationCounter <= 60)
        {
            permitTypeId = 4;
        }
        else if (registrationCounter > 60 && registrationCounter <= 70)
        {
            permitTypeId = 5;
        }

        var RegistrationMaterial = new RegistrationMaterial
        {
            Id = ++registrationMaterialId,
            MaterialId = j,
            StatusID = null,
            RegistrationId = registrationId,
            DeterminationDate = DateTime.UtcNow,
            ReferenceNumber = $"REF{registrationCounter:D4}-{j:D2}",
            Comments = $"Test description for material {j} in registration {registrationCounter}",
            PermitTypeId = permitTypeId,
            PPCPermitNumber = $"PPC{registrationCounter:D4}-{j:D2}",
            WasteManagementLicenceNumber = $"WML{registrationCounter:D4}-{j:D2}",
            EnvironmentalPermitWasteManagementNumber = $"EWM{registrationCounter:D4}-{j:D2}",
            InstallationPermitNumber = $"IP{registrationCounter:D4}-{j:D2}",
            MaximumProcessingCapacityTonnes = 1000,
            PPCPeriodId = 1,
            WasteManagementPeriodId = 1,
            InstallationPeriodId = 1,
            EnvironmentalPermitWasteManagementPeriodId = 1,
            PPCReprocessingCapacityTonne = 2000,
            WasteManagementReprocessingCapacityTonne = 3000,
            InstallationReprocessingTonne = 4000,
            EnvironmentalPermitWasteManagementTonne = 5000,
            MaximumReprocessingCapacityTonne = 6000,
            MaximumReprocessingPeriodID = 1
        };

        if (registrationCounter <= 10)
        {
            materialExemptionReferences.AddRange(new List<MaterialExemptionReference>
                {
                    new MaterialExemptionReference
                    {
                        Id = ++materialExemptionReferenceId,
                        ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                        RegistrationMaterialId = registrationMaterialId
                    }
                });
        }
        else if (registrationCounter > 10 && registrationCounter <= 20)
        {
            materialExemptionReferences.AddRange(new List<MaterialExemptionReference>
            {
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                },
                new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}",
                    RegistrationMaterialId = registrationMaterialId
                }
            });
        }

        return RegistrationMaterial;
    }

    int FileUploadId = 1;
    private List<FileUpload> GetFileUploads(int registrationCounter, int j, int registrationMaterialId)
    {
        if (registrationCounter <= 50)
        {

            return new List<FileUpload>{
                    new FileUpload
                    {
                        Id = FileUploadId++,
                        RegistrationMaterialId = registrationMaterialId,
                        Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                        FileUploadTypeId = 1,
                        FileUploadStatusId = 1,
                        DateUploaded = DateTime.UtcNow,
                        UpdatedBy = "Test User",
                        Comments = "Test comment",
                        FileId = Guid.NewGuid().ToString()
                    }
                };
        }
        else if (registrationCounter > 50 && registrationCounter <= 90)
        {
            return new List<FileUpload>
            {
            };
        }
        else
        {
            return new List<FileUpload>
            {
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                },
                new FileUpload
                {
                    Id = FileUploadId++,
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-1.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = Guid.NewGuid().ToString()
                }
            };
        }
    }
    int RegistrationReprocessingIOId = 1;
    private RegistrationReprocessingIO GetReprocessionIos(int registrationCounter, int registrationMaterialId)
    {
        return new RegistrationReprocessingIO
        {
            Id = RegistrationReprocessingIOId++,
            RegistrationMaterialId = registrationMaterialId,
            ContaminantsTonne = 1,
            NonUKPackagingWasteTonne = 2,
            NotPackingWasteTonne = 3,
            ProcessLossTonne = 4,
            ReprocessingPackagingWasteLastYearFlag = registrationCounter <= 50,
            SenttoOtherSiteTonne = 5,
            UKPackagingWasteTonne = 6,
            TotalInput = 7,
            TotalOutput = 8,
            PlantEquipmentUsed = "shredder",
            TypeOfSupplier = "Shed"
        };
    }

    public virtual DbSet<Registration> Registrations { get; set; }
    public virtual DbSet<RegistrationMaterial> RegistrationMaterials { get; set; }
    public virtual DbSet<MaterialExemptionReference> MaterialExemptionReferences { get; set; }
    public virtual DbSet<RegistrationReprocessingIO> RegistrationReprocessingIO { get; set; }

    public virtual DbSet<RegulatorApplicationTaskStatus> RegulatorApplicationTaskStatus { get; set; }
    public virtual DbSet<RegulatorRegistrationTaskStatus> RegulatorRegistrationTaskStatus { get; set; }
    public DbSet<LookupMaterial> LookupMaterials { get; set; }
    public DbSet<LookupRegistrationMaterialStatus> LookupRegistrationMaterialStatuses { get; set; }
    public DbSet<LookupRegulatorTask> LookupTasks { get; set; }
    public DbSet<LookupRegistrationStatus> LookupRegistrationStatuses { get; set; }
    public DbSet<LookupTaskStatus> LookupTaskStatuses { get; set; }
    public DbSet<LookupAddress> LookupAddresses { get; set; }
    public DbSet<LookupPeriod> LookupPeriod { get; set; }
    public DbSet<LookupMaterialPermit> LookupMaterialPermit { get; set; }
}