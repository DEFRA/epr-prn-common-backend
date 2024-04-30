using EPR.Accreditation.API.Common.Data.DataModels;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using EPR.Accreditation.API.Common.Data.SeedData;
using EPR.Accreditation.SeedData;
using Microsoft.EntityFrameworkCore;

namespace EPR.Accreditation.API.Common.Data
{
    public class AccreditationContext : DbContext
    {
        public AccreditationContext()
        {
        }

        public AccreditationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataModels.Accreditation>()
                .HasIndex(a => a.ExternalId)
                .IsUnique();

            modelBuilder.Entity<DataModels.Accreditation>()
                .HasIndex(a => a.ReferenceNumber)
                .IsUnique();

            //modelBuilder.Entity<Site>()
            //    .HasIndex(s => s.ExternalId)
            //    .IsUnique();

            //modelBuilder.Entity<Site>()
            //    .HasIndex(s => new { s.Postcode, s.OrganisationId })
            //    .IsUnique();

            modelBuilder.Entity<WastePermit>()
                .HasIndex(w => w.AccreditationId)
                .IsUnique();

            modelBuilder.Entity<OverseasReprocessingSite>()
                .HasIndex(w => w.OverseasAddressId)
                .IsUnique();

            modelBuilder.Entity<OverseasContactPerson>()
                .HasIndex(oa => oa.OverseasAddressId)
                .IsUnique();

            modelBuilder.Entity<OverseasContactPerson>()
                .HasIndex(oa => oa.OverseasReprocessingSiteId)
                .IsUnique();

            modelBuilder.Entity<AccreditationMaterial>()
                .HasIndex(m => m.ExternalId)
                .IsUnique();

            modelBuilder.Entity<MaterialReprocessorDetails>()
                .HasIndex(d => d.AccreditationMaterialId)
                .IsUnique();

            modelBuilder.Entity<SaveAndComeBack>()
                .HasIndex(s => s.AccreditationId)
                .IsUnique();

            // seed the lookup tables
            InitialDataSeed.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<DataModels.Accreditation> Accreditation { get; set; }

        public virtual DbSet<AccreditationMaterial> AccreditationMaterial { get; set; }

        public virtual DbSet<AccreditationTaskProgress> AccreditationTaskProgress { get; set; }

        public virtual DbSet<AccreditationTaskProgressMaterial> AccreditationTaskProgressMaterial { get; set; }

        public virtual DbSet<ExemptionReference> ExemptionReference { get; set; }

        public virtual DbSet<FileUpload> FileUpload { get; set; }

        public virtual DbSet<MaterialReprocessorDetails> MaterialReprocessorDetails { get; set; }

        public virtual DbSet<OverseasAddress> OverseasAddress { get; set; }

        public virtual DbSet<OverseasContactPerson> OverseasContactPerson { get; set; }

        public virtual DbSet<OverseasReprocessingSite> OverseasReprocessingSite { get; set; }

        public virtual DbSet<ReprocessorSupportingInformation> ReprocessorSupportingInformation { get; set; }

        public virtual DbSet<Site> Site { get; set; }

        public virtual DbSet<SiteAuthority> SiteAuthority { get; set; }

        public virtual DbSet<WastePermit> WastePermit { get; set; }

        public virtual DbSet<WasteCode> WasteCodes { get; set; }

        public virtual DbSet<SaveAndComeBack> SaveAndComeBack { get; set; }

        public virtual DbSet<Material> Material { get; set; }

        public virtual DbSet<Address> Address { get; set; }

        #region Lookups
        public virtual DbSet<AccreditationStatus> AccreditationStatus { get; set; }

        public virtual DbSet<Country> Country { get; set; }

        public virtual DbSet<FileUploadStatus> FileUploadStatus { get; set; }

        public virtual DbSet<FileUploadType> FileUploadType { get; set; }

        public virtual DbSet<OperatorType> OperatorType { get; set; }

        public virtual DbSet<ReprocessorSupportingInformationType> ReprocessorSupportingInformationType { get; set; }

        public virtual DbSet<TaskName> TaskName { get; set; }

        public virtual DbSet<DataModels.Lookups.TaskStatus> TaskStatus { get; set; }

        public virtual DbSet<WasteCodeType> WasteCodeType { get; set; }

        public virtual DbSet<OverseasPersonType> OverseasPersonType { get; set; }
        #endregion
    }
}
