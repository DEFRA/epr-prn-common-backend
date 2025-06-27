using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPR.PRN.Backend.Data;

[ExcludeFromCodeCoverage]
public class EprContext : DbContext
{
    private readonly IConfiguration _configuration;

    public EprContext()
    {
       
    }

    public EprContext(DbContextOptions<EprContext> options)
        : base(options)
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

        modelBuilder.Entity<Registration>()
            .HasOne(r => r.CarrierBrokerDealerPermit)
            .WithOne()
            .HasForeignKey<CarrierBrokerDealerPermits>(cb => cb.RegistrationId);

        modelBuilder.Entity<CarrierBrokerDealerPermits>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

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

        modelBuilder.Entity<ObligationCalculationOrganisationSubmitterType>(entity =>
        {
            entity.HasIndex(a => a.TypeName)
            .IsUnique();

            entity.HasData
            (
                new ObligationCalculationOrganisationSubmitterType
                {
                    Id = 1,
                    TypeName = ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme.ToString()
                },
                new ObligationCalculationOrganisationSubmitterType
                {
                    Id = 2,
                    TypeName = ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant.ToString()
					}
            );
        });

			modelBuilder.Entity<ObligationCalculation>()
			.HasOne(c => c.Material)
			.WithMany()
			.HasForeignKey(c => c.MaterialId)
			.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ObligationCalculation>()
			.HasOne(c => c.ObligationCalculationOrganisationSubmitterType)
			.WithMany()
			.HasForeignKey(c => c.SubmitterTypeId)
			.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<LookupMaterial>().HasData(
                new LookupMaterial { Id = 1, MaterialName = "Plastic", MaterialCode = "PL" },
                new LookupMaterial { Id = 2, MaterialName = "Steel", MaterialCode = "ST" },
                new LookupMaterial { Id = 3, MaterialName = "Aluminium", MaterialCode = "AL" },
                new LookupMaterial { Id = 4, MaterialName = "Glass", MaterialCode = "GL" },
                new LookupMaterial { Id = 5, MaterialName = "Paper/Board", MaterialCode = "PA" },
                new LookupMaterial { Id = 6, MaterialName = "Wood", MaterialCode = "WO" });

            modelBuilder.Entity<LookupCountry>().HasData(
                CountryConstants.Countries.Select(c => new LookupCountry { Id = c.Id, CountryCode = c.Code, Name = c.Name }).ToArray()
            );

        modelBuilder.Entity<LookupRegistrationMaterialStatus>().HasData(
            new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            new LookupRegistrationMaterialStatus { Id = 2, Name = "Refused" },
            new LookupRegistrationMaterialStatus { Id = 3, Name = "Started" },
            new LookupRegistrationMaterialStatus { Id = 4, Name = "Submitted" },
            new LookupRegistrationMaterialStatus { Id = 5, Name = "RegulatorReviewing" },
            new LookupRegistrationMaterialStatus { Id = 6, Name = "Queried" },
            new LookupRegistrationMaterialStatus { Id = 8, Name = "Withdrawn" },
            new LookupRegistrationMaterialStatus { Id = 9, Name = "Suspended" },
            new LookupRegistrationMaterialStatus { Id = 10, Name = "Cancelled" },
            new LookupRegistrationMaterialStatus { Id = 11, Name = "ReadyToSubmit" });

        modelBuilder.Entity<LookupAccreditationStatus>().HasData(
            new LookupAccreditationStatus { Id = 1, Name = "Started" },
            new LookupAccreditationStatus { Id = 2, Name = "Submitted" },
            new LookupAccreditationStatus { Id = 3, Name = "RegulatorReviewing" },
            new LookupAccreditationStatus { Id = 4, Name = "Queried" },
            new LookupAccreditationStatus { Id = 5, Name = "Updated" },
            new LookupAccreditationStatus { Id = 6, Name = "Granted" },
            new LookupAccreditationStatus { Id = 7, Name = "Refused" },
            new LookupAccreditationStatus { Id = 8, Name = "Withdrawn" },
            new LookupAccreditationStatus { Id = 9, Name = "Suspended" },
            new LookupAccreditationStatus { Id = 10, Name = "Cancelled" },
            new LookupAccreditationStatus { Id = 11, Name = "ReadyToSubmit" });

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
            new LookupRegulatorTask { Id = 1, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.SiteAddressAndContactDetails },
            new LookupRegulatorTask { Id = 2, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.MaterialsAuthorisedOnSite },
            new LookupRegulatorTask { Id = 3, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.RegistrationDulyMade },
            new LookupRegulatorTask { Id = 4, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.WasteLicensesPermitsAndExemptions },
            new LookupRegulatorTask { Id = 5, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.ReprocessingInputsAndOutputs },
            new LookupRegulatorTask { Id = 6, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.SamplingAndInspectionPlan },
            new LookupRegulatorTask { Id = 7, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.AssignOfficer },
            new LookupRegulatorTask { Id = 8, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.BusinessAddress },
            new LookupRegulatorTask { Id = 9, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.WasteLicensesPermitsAndExemptions },
            new LookupRegulatorTask { Id = 10, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.RegistrationDulyMade },
            new LookupRegulatorTask { Id = 11, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.SamplingAndInspectionPlan },
            new LookupRegulatorTask { Id = 12, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.AssignOfficer },
            new LookupRegulatorTask { Id = 13, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.MaterialDetailsAndContact },
            new LookupRegulatorTask { Id = 14, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.OverseasReprocessorAndInterimSiteDetails },
            new LookupRegulatorTask { Id = 15, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.CheckRegistrationStatus },
            new LookupRegulatorTask { Id = 16, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 1, Name = RegulatorTaskNames.CheckRegistrationStatus },
            new LookupRegulatorTask { Id = 17, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 2, Name = RegulatorTaskNames.AssignOfficer },
            new LookupRegulatorTask { Id = 18, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 2, Name = RegulatorTaskNames.PrnsTonnageAndAuthorityToIssuePrns },
            new LookupRegulatorTask { Id = 19, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 2, Name = RegulatorTaskNames.BusinessPlan },
            new LookupRegulatorTask { Id = 20, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 2, Name = RegulatorTaskNames.AccreditationSamplingAndInspectionPlan },

            new LookupRegulatorTask { Id = 22, IsMaterialSpecific = false, ApplicationTypeId = 2, JourneyTypeId = 2, Name = RegulatorTaskNames.AssignOfficer },
            new LookupRegulatorTask { Id = 23, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 2, Name = RegulatorTaskNames.PERNsTonnageAndAuthorityToIssuePERNs },
            new LookupRegulatorTask { Id = 24, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 2, Name = RegulatorTaskNames.BusinessPlan },
            new LookupRegulatorTask { Id = 25, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 2, Name = RegulatorTaskNames.AccreditationSamplingAndInspectionPlan },
            new LookupRegulatorTask { Id = 26, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 2, Name = RegulatorTaskNames.OverseasReprocessingSitesAndBroadlyEquivalentEvidence },
            new LookupRegulatorTask { Id = 27, IsMaterialSpecific = true, ApplicationTypeId = 1, JourneyTypeId = 2, Name = RegulatorTaskNames.DulyMade },
            new LookupRegulatorTask { Id = 28, IsMaterialSpecific = true, ApplicationTypeId = 2, JourneyTypeId = 2, Name = RegulatorTaskNames.DulyMade },
            new LookupRegulatorTask { Id = 29, IsMaterialSpecific = false, ApplicationTypeId = 1, JourneyTypeId = 1, Name = RegulatorTaskNames.WasteCarrierBrokerDealerNumber });

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

        modelBuilder.Entity<Accreditation>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<AccreditationDeterminationDate>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<AccreditationDulyMade>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<DeterminationDate>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<MaterialExemptionReference>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<Registration>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<RegistrationMaterial>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<RegistrationReprocessingIO>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<ApplicantRegistrationTaskStatus>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<RegulatorAccreditationRegistrationTaskStatus>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<RegulatorAccreditationTaskStatus>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<RegulatorApplicationTaskStatus>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        modelBuilder.Entity<RegulatorRegistrationTaskStatus>()
            .HasIndex(e => e.ExternalId)
            .IsUnique(); // Ensures UniqueId is unique

        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Eprn> Prn { get; set; }

    public virtual DbSet<PrnStatus> PrnStatus { get; set; }

    public virtual DbSet<PrnStatusHistory> PrnStatusHistory { get; set; }

    public virtual DbSet<RecyclingTarget> RecyclingTargets { get; set; }

    public virtual DbSet<ObligationCalculation> ObligationCalculations { get; set; }

    public virtual DbSet<Material> Material { get; set; }

		public virtual DbSet<ObligationCalculationOrganisationSubmitterType> ObligationCalculationOrganisationSubmitterType { get; set; }

		public virtual DbSet<PEprNpwdSync> PEprNpwdSync { get; set; }

    public virtual DbSet<PrnMaterialMapping> PrnMaterialMapping { get; set; }

        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<RegistrationMaterial> RegistrationMaterials { get; set; }
        public virtual DbSet<MaterialExemptionReference> MaterialExemptionReferences { get; set; }
        public virtual DbSet<RegistrationReprocessingIO> RegistrationReprocessingIO { get; set; }
        public virtual DbSet<DeterminationDate> DeterminationDate { get; set; }
        public virtual DbSet<DulyMade> DulyMade { get; set; }
        public virtual DbSet<CarrierBrokerDealerPermits> CarrierBrokerDealerPermits { get; set; }
        public virtual DbSet<RegulatorApplicationTaskStatus> RegulatorApplicationTaskStatus { get; set; }
        public virtual DbSet<RegulatorRegistrationTaskStatus> RegulatorRegistrationTaskStatus { get; set; }
        public virtual DbSet<ApplicantRegistrationTaskStatus> RegistrationTaskStatus { get; set; }
        public virtual DbSet<LookupMaterial> LookupMaterials { get; set; }
        public virtual DbSet<LookupRegistrationMaterialStatus> LookupRegistrationMaterialStatuses { get; set; }
        public virtual DbSet<LookupRegulatorTask> LookupTasks { get; set; }
        public virtual DbSet<LookupTaskStatus> LookupTaskStatuses { get; set; }
        public virtual DbSet<Address> LookupAddresses { get; set; }
        public virtual DbSet<LookupPeriod> LookupPeriod { get; set; }
        public virtual DbSet<LookupMaterialPermit> LookupMaterialPermit { get; set; }
        public virtual DbSet<Note> QueryNote { get; set; }
        public virtual DbSet<ApplicationTaskStatusQueryNote> ApplicationTaskStatusQueryNotes { get; set; }
        public virtual DbSet<RegistrationTaskStatusQueryNote> RegistrationTaskStatusQueryNotes { get; set; }
        public virtual DbSet<Accreditation> Accreditations { get; set; }
        public virtual DbSet<AccreditationDulyMade> AccreditationDulyMade { get; set; }
        public virtual DbSet<RegulatorAccreditationTaskStatus> RegulatorAccreditationTaskStatus { get; set; }
        public virtual DbSet<AccreditationTaskStatusQueryNote> AccreditationTaskStatusQueryNote { get; set; }
        public virtual DbSet<AccreditationDeterminationDate> AccreditationDeterminationDate { get; set; }
        public virtual DbSet<LookupCountry> LookupCountries { get; set; }

}