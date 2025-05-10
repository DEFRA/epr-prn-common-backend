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
    public class EprContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EprContext()
        {

        }

        public EprContext(DbContextOptions<EprContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("EprConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Eprn>()
                .HasIndex(a => a.ExternalId)
                .IsUnique();

            modelBuilder.Entity<Eprn>()
                .HasIndex(a => a.PrnNumber)
                .IsUnique();

            modelBuilder.Entity<PrnStatus>()
                .HasData(DataModels.PrnStatus.Data);

            modelBuilder.Entity<RecyclingTarget>()
                .HasData
                (
                    // Paper
                    new() { Id = 1, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.75, Year = 2025 },
                    new() { Id = 2, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.77, Year = 2026 },
                    new() { Id = 3, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.79, Year = 2027 },
                    new() { Id = 4, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.81, Year = 2028 },
                    new() { Id = 5, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.83, Year = 2029 },
                    new() { Id = 6, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.85, Year = 2030 },

                    // Glass
                    new() { Id = 7, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.74, Year = 2025 },
                    new() { Id = 8, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.76, Year = 2026 },
                    new() { Id = 9, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.78, Year = 2027 },
                    new() { Id = 10, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.80, Year = 2028 },
                    new() { Id = 11, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.82, Year = 2029 },
                    new() { Id = 12, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.85, Year = 2030 },

                    // Aluminium
                    new() { Id = 13, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.61, Year = 2025 },
                    new() { Id = 14, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.62, Year = 2026 },
                    new() { Id = 15, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.63, Year = 2027 },
                    new() { Id = 16, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.64, Year = 2028 },
                    new() { Id = 17, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.65, Year = 2029 },
                    new() { Id = 18, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.67, Year = 2030 },

                    // Steel
                    new() { Id = 19, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.80, Year = 2025 },
                    new() { Id = 20, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.81, Year = 2026 },
                    new() { Id = 21, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.82, Year = 2027 },
                    new() { Id = 22, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.83, Year = 2028 },
                    new() { Id = 23, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.84, Year = 2029 },
                    new() { Id = 24, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.85, Year = 2030 },

                    // Plastic
                    new() { Id = 25, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.55, Year = 2025 },
                    new() { Id = 26, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.57, Year = 2026 },
                    new() { Id = 27, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.59, Year = 2027 },
                    new() { Id = 28, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.61, Year = 2028 },
                    new() { Id = 29, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.63, Year = 2029 },
                    new() { Id = 30, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.65, Year = 2030 },

                    // Wood
                    new() { Id = 31, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.45, Year = 2025 },
                    new() { Id = 32, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.46, Year = 2026 },
                    new() { Id = 33, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.47, Year = 2027 },
                    new() { Id = 34, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.48, Year = 2028 },
                    new() { Id = 35, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.49, Year = 2029 },
                    new() { Id = 36, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.50, Year = 2030 },

                    // Glass Remelt
                    new() { Id = 37, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.75, Year = 2025 },
                    new() { Id = 38, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.76, Year = 2026 },
                    new() { Id = 39, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.77, Year = 2027 },
                    new() { Id = 40, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.78, Year = 2028 },
                    new() { Id = 41, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.79, Year = 2029 },
                    new() { Id = 42, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.80, Year = 2030 },

                    // Fibre Composite
                    new() { Id = 43, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.75, Year = 2025 },
                    new() { Id = 44, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.77, Year = 2026 },
                    new() { Id = 45, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.79, Year = 2027 },
                    new() { Id = 46, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.81, Year = 2028 },
                    new() { Id = 47, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.83, Year = 2029 },
                    new() { Id = 48, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.85, Year = 2030 }
                );

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasMany(material => material.PrnMaterialMappings)
                .WithOne()
                .HasForeignKey(s => s.PRNMaterialId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(a => a.MaterialCode)
                .IsUnique();

                entity.HasData
				(
					new Material { Id = 1, MaterialCode = "PL", MaterialName = MaterialType.Plastic.ToString() },
					new Material { Id = 2, MaterialCode = "WD", MaterialName = MaterialType.Wood.ToString() },
					new Material { Id = 3, MaterialCode = "AL", MaterialName = MaterialType.Aluminium.ToString() },
					new Material { Id = 4, MaterialCode = "ST", MaterialName = MaterialType.Steel.ToString() },
					new Material { Id = 5, MaterialCode = "PC", MaterialName = MaterialType.Paper.ToString() },
					new Material { Id = 6, MaterialCode = "GL", MaterialName = MaterialType.Glass.ToString() },
					new Material { Id = 7, MaterialCode = "GR", MaterialName = MaterialType.GlassRemelt.ToString() },
					new Material { Id = 8, MaterialCode = "FC", MaterialName = MaterialType.FibreComposite.ToString() }
				);
			});

			modelBuilder.Entity<PrnMaterialMapping>()
				.HasData
				(
					new PrnMaterialMapping { Id = 1, PRNMaterialId = 1, NPWDMaterialName = PrnConstants.Materials.Plastic },
					new PrnMaterialMapping { Id = 2, PRNMaterialId = 2, NPWDMaterialName = PrnConstants.Materials.Wood },
					new PrnMaterialMapping { Id = 3, PRNMaterialId = 2, NPWDMaterialName = PrnConstants.Materials.WoodComposting },
					new PrnMaterialMapping { Id = 4, PRNMaterialId = 3, NPWDMaterialName = PrnConstants.Materials.Aluminium },
					new PrnMaterialMapping { Id = 5, PRNMaterialId = 4, NPWDMaterialName = PrnConstants.Materials.Steel },
					new PrnMaterialMapping { Id = 6, PRNMaterialId = 5, NPWDMaterialName = PrnConstants.Materials.PaperFiber },
					new PrnMaterialMapping { Id = 7, PRNMaterialId = 5, NPWDMaterialName = PrnConstants.Materials.PaperComposting },
					new PrnMaterialMapping { Id = 8, PRNMaterialId = 6, NPWDMaterialName = PrnConstants.Materials.GlassOther },
					new PrnMaterialMapping { Id = 9, PRNMaterialId = 7, NPWDMaterialName = PrnConstants.Materials.GlassMelt }
				);

			modelBuilder.Entity<Eprn>(entity =>
            {
                entity.HasMany(prn => prn.PrnStatusHistories)
                .WithOne()
                .HasForeignKey(s => s.PrnIdFk)
                .OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<ObligationCalculation>()
			.HasOne(c => c.Material)
			.WithMany()
			.HasForeignKey(c => c.MaterialId)
			.OnDelete(DeleteBehavior.NoAction);

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

                SeedData(modelBuilder);

                base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Eprn> Prn { get; set; }

        public virtual DbSet<PrnStatus> PrnStatus { get; set; }

        public virtual DbSet<PrnStatusHistory> PrnStatusHistory { get; set; }

        public virtual DbSet<RecyclingTarget> RecyclingTargets { get; set; }

        public virtual DbSet<ObligationCalculation> ObligationCalculations { get; set; }

        public virtual DbSet<Material> Material { get; set; }

        public virtual DbSet<PEprNpwdSync> PEprNpwdSync { get; set; }

        public virtual DbSet<PrnMaterialMapping> PrnMaterialMapping { get; set; }

        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationMaterial> RegistrationMaterials { get; set; }
        public virtual DbSet<MaterialExemptionReference> MaterialExemptionReferences { get; set; }
        public virtual DbSet<RegistrationReprocessingIO> RegistrationReprocessingIO { get; set; }

        public virtual DbSet<RegulatorApplicationTaskStatus> RegulatorApplicationTaskStatus { get; set; }
        public virtual DbSet<RegulatorRegistrationTaskStatus> RegulatorRegistrationTaskStatus { get; set; }
        public virtual DbSet<LookupMaterial> LookupMaterials { get; set; }
        public virtual DbSet<LookupRegistrationMaterialStatus> LookupRegistrationMaterialStatuses { get; set; }
        public virtual DbSet<LookupRegulatorTask> LookupTasks { get; set; }
        public virtual DbSet<LookupTaskStatus> LookupTaskStatuses { get; set; }
        public virtual DbSet<Address> LookupAddresses { get; set; }
        public virtual DbSet<LookupPeriod> LookupPeriod { get; set; }
        public virtual DbSet<LookupMaterialPermit> LookupMaterialPermit { get; set; }

        int NumberOfRegistrations = 100;
        int registrationId = 0;
        int lookupAddressID = 0;
        int registrationMaterialId = 0;
        int materialExemptionReferenceId = 0;
       
        private void SeedData(ModelBuilder modelBuilder)
        {
            

            var lookupAddresses = new List<Address>();
            var registrations = new List<Registration>();
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

            var addressTemplates = new[]
            {
                new { AddressLine1 = "23", AddressLine2 = "Ruby St", TownCity = "London", Country = "England",        PostCode = "E12 3SE", NationId = 1, County = (string?)null },
                new { AddressLine1 = "78", AddressLine2 = "Pine Ln", TownCity = "Belfast", Country = "Northern Ireland", PostCode = "BT1 3FG", NationId = 2, County = (string?)null },
                new { AddressLine1 = "45", AddressLine2 = "Maple Ave", TownCity = "Edinburgh", Country = "Scotland",     PostCode = "EH3 5DN", NationId = 3, County = (string?)null },
                new { AddressLine1 = "12", AddressLine2 = "Oak Rd", TownCity = "Cardiff", Country = "Wales",         PostCode = "CF10 1AA", NationId = 4, County = (string?)null }
            };

            for (var registrationCounter = 1; registrationCounter <= NumberOfRegistrations; registrationCounter++)
            {
                var ApplicationTypeId = registrationCounter % 2 + 1;
                int currentAddressId = 0;

                int currentRegId = ++registrationId;

                //if (ApplicationTypeId == 2)
                //{
                    var templateIndex = ((currentRegId /2)% addressTemplates.Length);

                    var template = addressTemplates[templateIndex];

                    lookupAddresses.Add(new Address
                    {
                        Id = ++lookupAddressID,
                        AddressLine1 = template.AddressLine1,
                        AddressLine2 = template.AddressLine2,
                        TownCity = template.TownCity,
                        County = template.County,
                        PostCode = template.PostCode,
                        NationId = template.NationId,
                        GridReference = $"SJ 854 66{registrationCounter}"
                    });

                    currentAddressId = lookupAddressID;
                //}

                registrations.Add(new Registration
                {
                    Id = currentRegId,
                    ExternalId = Guid.NewGuid(),
                    ApplicationTypeId = ApplicationTypeId,
                    OrganisationId = 1,
                    BusinessAddressId = ApplicationTypeId == 2 ? currentAddressId : null,
                    ReprocessingSiteAddressId = ApplicationTypeId == 1 ? currentAddressId : null,
                    LegalDocumentAddressId = currentAddressId,
                });

                for (int j = 1; j <= 3; j++)
                {
                    registrationMaterials.Add(GetRegistrationMaterial(registrationCounter, currentRegId, j, materialExemptionReferences));
                    registrationReprocessingIOs.Add(GetReprocessionIos(registrationCounter, registrationMaterialId));
                    fileUploads.AddRange(GetFileUploads(registrationCounter, j, registrationMaterialId));
                }
            }

            modelBuilder.Entity<Registration>().HasData(registrations);
            modelBuilder.Entity<Address>().HasData(lookupAddresses);
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

            // bool isRegistered = new Random().Next(2) == 0;

            bool isRegistered = GetIsRegistered(registrationCounter, j);

            var RegistrationMaterial = new RegistrationMaterial
            {
                Id = ++registrationMaterialId,
                ExternalId = Guid.NewGuid(),
                MaterialId = j,
                StatusId = null,
                RegistrationId = registrationId,
                ReferenceNumber = $"REF{registrationCounter:D4}-{j:D2}",
                Comments = $"Test description for material {j} in registration {registrationCounter}",
                PermitTypeId = permitTypeId,
                PPCPermitNumber = $"PPC{registrationCounter:D4}-{j:D2}",
                WasteManagementLicenceNumber = $"WML{registrationCounter:D4}-{j:D2}",
                EnvironmentalPermitWasteManagementNumber = $"EWM{registrationCounter:D4}-{j:D2}",
                InstallationPermitNumber = $"IP{registrationCounter:D4}-{j:D2}",
                PPCPeriodId = 1,
                WasteManagementPeriodId = 1,
                InstallationPeriodId = 1,
                EnvironmentalPermitWasteManagementPeriodId = 1,
                PPCReprocessingCapacityTonne = 2000,
                WasteManagementReprocessingCapacityTonne = 3000,
                InstallationReprocessingTonne = 4000,
                EnvironmentalPermitWasteManagementTonne = 5000,
                MaximumReprocessingCapacityTonne = 6000,
                MaximumReprocessingPeriodId = 1,
                ReasonforNotreg = isRegistered ? string.Empty : $"Lorem ipsum dolor sit amet, consectetur adipiscing{j} elit. Fusce vulputate aliquet ornare. Vestibulum dolor nunc, tincidunt a diam nec, mattis venenatis sem{registrationCounter}",
                //Wastecarrierbrokerdealerregistration = $"DFG3457345{registrationCounter}",
                IsMaterialRegistered = isRegistered
            };

            if (registrationCounter <= 10)
            {
                materialExemptionReferences.AddRange(GetMaterialExemptionReferences(registrationCounter, j, registrationMaterialId, 1));
            }
            else if (registrationCounter > 10 && registrationCounter <= 20)
            {
                materialExemptionReferences.AddRange(GetMaterialExemptionReferences(registrationCounter, j, registrationMaterialId, 10));
            }

            return RegistrationMaterial;
        }
        private bool GetIsRegistered(int registrationCounter, int j)
        {
            if (registrationCounter <= 10)
            {
                return true;
            }
            var registrationRules = new List<(int min, int max, HashSet<int> allowed)>
                        {
                            (11, 20, new HashSet<int> {1, 2}),
                            (21, 40, new HashSet<int> {1, 3}),
                            (41, 60, new HashSet<int> {2, 3}),
                            (61, 70, new HashSet<int> {1, 3}),
                            (71, 80, new HashSet<int> {1}),
                            (81, 90, new HashSet<int> {2}),
                            (91, 99, new HashSet<int> {3}),
                        };
            foreach (var rule in registrationRules)
            {
                if (registrationCounter >= rule.min && registrationCounter <= rule.max)
                {
                    return rule.allowed.Contains(j);
                }
            }

            if (registrationCounter == 100)
            {
                return false;
            }
            return false;
        }

        private List<MaterialExemptionReference> GetMaterialExemptionReferences(int registrationCounter, int j, int registrationMaterialId, int NumberOfMaterialExemptionReferences)
        {
            var materialExemptionReferences = new List<MaterialExemptionReference>();
            for (int i = 0; i < NumberOfMaterialExemptionReferences; i++)
            {
                materialExemptionReferences.Add(new MaterialExemptionReference
                {
                    Id = ++materialExemptionReferenceId,
                    ExternalId = Guid.NewGuid(),
                    ReferenceNo = $"EXEMPT{registrationCounter:D4}-{materialExemptionReferenceId:D2}-{i}",
                    RegistrationMaterialId = registrationMaterialId
                });
            }
            return materialExemptionReferences;
        }

        int FileUploadId = 1;
        private List<FileUpload> GetFileUploads(int registrationCounter, int j, int registrationMaterialId)
        {
            if (registrationCounter <= 50)
            {
                return GetFileUploads(registrationCounter, j, registrationMaterialId, 1);
            }
            else if (registrationCounter > 50 && registrationCounter <= 90)
            {
                return new List<FileUpload>();
            }
            else
            {
                return GetFileUploads(registrationCounter, j, registrationMaterialId, 10);
            }
        }

        private List<FileUpload> GetFileUploads(int registrationCounter, int j, int registrationMaterialId, int NumberOfFileUploads)
        {
            var fileUploads = new List<FileUpload>();
            for (int i = 0; i < NumberOfFileUploads; i++)
            {
                fileUploads.Add(new FileUpload
                {
                    Id = FileUploadId++,
                    ExternalId = Guid.NewGuid(),
                    RegistrationMaterialId = registrationMaterialId,
                    Filename = $"File{registrationCounter:D4}-{j:D2}-{i}.pdf",
                    FileUploadTypeId = 1,
                    FileUploadStatusId = 1,
                    DateUploaded = DateTime.UtcNow,
                    UpdatedBy = Guid.NewGuid(),
                    Comments = "Test comment",
                    FileId = Guid.NewGuid()
                });
            }
            return fileUploads;
        }

        int RegistrationReprocessingIOId = 1;
        private RegistrationReprocessingIO GetReprocessionIos(int registrationCounter, int registrationMaterialId)
        {
            return new RegistrationReprocessingIO
            {
                Id = RegistrationReprocessingIOId++,
                ExternalId = Guid.NewGuid(),
                RegistrationMaterialId = registrationMaterialId,
                ContaminantsTonne = 1,
                NonUKPackagingWasteTonne = 2,
                NotPackingWasteTonne = 3,
                ProcessLossTonne = 4,
                ReprocessingPackagingWasteLastYearFlag = registrationCounter <= 50,
                SenttoOtherSiteTonne = 5,
                UKPackagingWasteTonne = 6,
                TotalInputs = 7,
                TotalOutputs = 8,
                PlantEquipmentUsed = "shredder",
                TypeOfSupplier = "Shed"
            };
        }
    }
}