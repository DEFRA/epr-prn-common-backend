using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data
{
    [ExcludeFromCodeCoverage]
    public class EprContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EprContext()
        {

        }

        public EprContext(DbContextOptions options) : base(options)
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

            modelBuilder.Entity<FeesAmount>()
                .HasIndex(a => a.MaterialId)
                .IsUnique();

            modelBuilder.Entity<RecyclingTarget>()
                .HasData(
                            // Paper
                            new RecyclingTarget { Id = 1, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.75, Year = 2025 },
                            new RecyclingTarget { Id = 2, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.77, Year = 2026 },
                            new RecyclingTarget { Id = 3, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.79, Year = 2027 },
                            new RecyclingTarget { Id = 4, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.81, Year = 2028 },
                            new RecyclingTarget { Id = 5, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.83, Year = 2029 },
                            new RecyclingTarget { Id = 6, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.85, Year = 2030 },

                            // Glass
                            new RecyclingTarget { Id = 7, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.74, Year = 2025 },
                            new RecyclingTarget { Id = 8, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.76, Year = 2026 },
                            new RecyclingTarget { Id = 9, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.78, Year = 2027 },
                            new RecyclingTarget { Id = 10, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.80, Year = 2028 },
                            new RecyclingTarget { Id = 11, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.82, Year = 2029 },
                            new RecyclingTarget { Id = 12, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.85, Year = 2030 },

                            // Aluminium
                            new RecyclingTarget { Id = 13, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.61, Year = 2025 },
                            new RecyclingTarget { Id = 14, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.62, Year = 2026 },
                            new RecyclingTarget { Id = 15, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.63, Year = 2027 },
                            new RecyclingTarget { Id = 16, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.64, Year = 2028 },
                            new RecyclingTarget { Id = 17, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.65, Year = 2029 },
                            new RecyclingTarget { Id = 18, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.67, Year = 2030 },

                            // Steel
                            new RecyclingTarget { Id = 19, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.80, Year = 2025 },
                            new RecyclingTarget { Id = 20, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.81, Year = 2026 },
                            new RecyclingTarget { Id = 21, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.82, Year = 2027 },
                            new RecyclingTarget { Id = 22, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.83, Year = 2028 },
                            new RecyclingTarget { Id = 23, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.84, Year = 2029 },
                            new RecyclingTarget { Id = 24, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.85, Year = 2030 },

                            // Plastic
                            new RecyclingTarget { Id = 25, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.55, Year = 2025 },
                            new RecyclingTarget { Id = 26, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.57, Year = 2026 },
                            new RecyclingTarget { Id = 27, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.59, Year = 2027 },
                            new RecyclingTarget { Id = 28, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.61, Year = 2028 },
                            new RecyclingTarget { Id = 29, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.63, Year = 2029 },
                            new RecyclingTarget { Id = 30, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.65, Year = 2030 },

                            // Wood
                            new RecyclingTarget { Id = 31, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.45, Year = 2025 },
                            new RecyclingTarget { Id = 32, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.46, Year = 2026 },
                            new RecyclingTarget { Id = 33, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.47, Year = 2027 },
                            new RecyclingTarget { Id = 34, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.48, Year = 2028 },
                            new RecyclingTarget { Id = 35, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.49, Year = 2029 },
                            new RecyclingTarget { Id = 36, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.50, Year = 2030 },

                            // Glass Remelt
                            new RecyclingTarget { Id = 37, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.75, Year = 2025 },
                            new RecyclingTarget { Id = 38, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.76, Year = 2026 },
                            new RecyclingTarget { Id = 39, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.77, Year = 2027 },
                            new RecyclingTarget { Id = 40, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.78, Year = 2028 },
                            new RecyclingTarget { Id = 41, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.79, Year = 2029 },
                            new RecyclingTarget { Id = 42, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.80, Year = 2030 },

                            // Fibre Composite
                            new RecyclingTarget { Id = 43, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.75, Year = 2025 },
                            new RecyclingTarget { Id = 44, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.77, Year = 2026 },
                            new RecyclingTarget { Id = 45, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.79, Year = 2027 },
                            new RecyclingTarget { Id = 46, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.81, Year = 2028 },
                            new RecyclingTarget { Id = 47, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.83, Year = 2029 },
                            new RecyclingTarget { Id = 48, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.85, Year = 2030 }
                        );

            modelBuilder.Entity<ApplicationType>()
                .HasData(
                            new ApplicationType { Name = "Reprocessor" },
                            new ApplicationType { Name = "Exporter" }
                );

            modelBuilder.Entity<Period>()
                .HasData(
                    new Period { Name = "Per Week" },
                    new Period { Name = "Per Month" },
                    new Period { Name = "Per Year" }
                );

            modelBuilder.Entity<MaterialPermitType>()
                .HasData(
                    new MaterialPermitType { Name = "Environmental permit or waste management licence" },
                    new MaterialPermitType { Name = "Installation Permit" },
                    new MaterialPermitType { Name = "Pollution, Prevention, and Control (PPC) permit" },
                    new MaterialPermitType { Name = "Waste Exemption" },
                    new MaterialPermitType { Name = "Waste Management Licence" }
                );

            modelBuilder.Entity<RegistrationStatus>()
                .HasData(
                    new RegistrationStatus { Name = "Accepted" },
                    new RegistrationStatus { Name = "Cancelled" },
                    new RegistrationStatus { Name = "Granted" },
                    new RegistrationStatus { Name = "Queried" },
                    new RegistrationStatus { Name = "Refused" },
                    new RegistrationStatus { Name = "Started" },
                    new RegistrationStatus { Name = "Submitted" },
                    new RegistrationStatus { Name = "Suspended" },
                    new RegistrationStatus { Name = "Updated" },
                    new RegistrationStatus { Name = "Withdrawn" }
                );

            modelBuilder.Entity<Material>()
                .HasData(
                            new Material { MaterialCode = "PL", MaterialName = "Plastic" },
                            new Material { MaterialCode = "WD", MaterialName = "Wood" },
                            new Material { MaterialCode = "AL", MaterialName = "Aluminium" },
                            new Material { MaterialCode = "ST", MaterialName = "Steel" },
                            new Material { MaterialCode = "PC", MaterialName = "Paper" },
                            new Material { MaterialCode = "GL", MaterialName = "Glass" },
                            new Material { MaterialCode = "FC", MaterialName = "FibreComposite" }
                );

            modelBuilder.Entity<Eprn>(entity =>
            {
                entity.HasMany(prn => prn.PrnStatusHistories)
                .WithOne()
                .HasForeignKey(s => s.PrnIdFk)
                .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<DataModels.TaskStatus>()
            .HasData(
                        new DataModels.TaskStatus { Name = "Not started" },
                        new DataModels.TaskStatus { Name = "Started" },
                        new DataModels.TaskStatus { Name = "Completed" },
                        new DataModels.TaskStatus { Name = "Cannot start yet" }
            );

            modelBuilder.Entity<DataModels.TaskName>()
           .HasData(
                       new DataModels.TaskStatus { Name = "SiteAddressAndContactDetails" },
                       new DataModels.TaskStatus { Name = "WasteLicensesPermitsAndExemption" },
                       new DataModels.TaskStatus { Name = "ReprocessingInputandOutput" },
                       new DataModels.TaskStatus { Name = "SamplingAndInspectionPlanPerMaterial" }
           );

            modelBuilder.Entity<DataModels.FileUploadType>()
          .HasData(
                      new DataModels.FileUploadType { Name = "SamplingAndInspectionPlan" }
          );

            modelBuilder.Entity<DataModels.FileUploadStatus>()
        .HasData(
                    new DataModels.FileUploadStatus { Name = "Virus check failed" },
                    new DataModels.FileUploadStatus { Name = "Virus check succeeded" },
                    new DataModels.FileUploadStatus { Name = "Upload complete" },
                    new DataModels.FileUploadStatus { Name = "Upload failed" },
                    new DataModels.FileUploadStatus { Name = "File deleted" }
        );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<AppRefPerMaterial> AppRefPerMaterial { get; set; }
        public DbSet<FeesAmount> FeesAmount { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<FileUploadStatus> FileUploadStatus { get; set; }
        public virtual DbSet<FileUploadType> FileUploadType { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public DbSet<MaterialPermitType> MaterialPermitType { get; set; }
        public virtual DbSet<ObligationCalculation> ObligationCalculations { get; set; }
        public virtual DbSet<PEprNpwdSync> PEprNpwdSync { get; set; }
        public DbSet<Period> Period { get; set; }
        public virtual DbSet<Eprn> Prn { get; set; }
        public virtual DbSet<PrnStatus> PrnStatus { get; set; }
        public virtual DbSet<PrnStatusHistory> PrnStatusHistory { get; set; }
        public virtual DbSet<RecyclingTarget> RecyclingTargets { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<RegistrationContact> RegistrationContact { get; set; }
        public virtual DbSet<RegistrationContact> RegistrationTaskStatus { get; set; }
        public virtual DbSet<RegistrationMaterial> RegistrationMaterial { get; set; }
        public virtual DbSet<RegistrationProcessingIORawMaterial> RegistrationProcessingIORawMaterial { get; set; }
        public virtual DbSet<RegistrationReprocessingIO> RegistrationReprocessingIO { get; set; }
        public virtual DbSet<RegistrationStatus> RegistrationStatus { get; set; }
        public virtual DbSet<SaveAndContinue> SaveAndContinue { get; set; }
        public virtual DbSet<TaskName> TaskName { get; set; }
        public virtual DbSet<TaskName> Taskstatus { get; set; }
    }
}