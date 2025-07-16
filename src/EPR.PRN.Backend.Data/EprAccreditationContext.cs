//using EPR.PRN.Backend.Data.DataModels.Accreditations;
//using EPR.PRN.Backend.Data.DataModels.Registrations;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics.CodeAnalysis;

//namespace EPR.PRN.Backend.Data;

//[ExcludeFromCodeCoverage]
//public class EprAccreditationContext : DbContext
//{
//    public EprAccreditationContext()
//    {
//    }

//    public EprAccreditationContext(DbContextOptions<EprAccreditationContext> options) : base(options)
//    {
//    }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        if (!optionsBuilder.IsConfigured)
//        {
//            optionsBuilder.UseInMemoryDatabase("EprAccreditationDatabase");
//        }
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<AccreditationEntity>(e =>
//        {
//            e.HasOne(x => x.ApplicationType)
//                .WithMany()
//                .HasForeignKey(x => x.ApplicationTypeId);

//            e.HasOne(x => x.AccreditationStatus)
//                .WithMany()
//                .HasForeignKey(x => x.AccreditationStatusId);

//            e.HasOne(x => x.RegistrationMaterial)
//                .WithMany()
//                .HasForeignKey(x => x.RegistrationMaterialId);

//            e.HasMany(x => x.AccreditationPrnIssueAuths)
//                .WithOne()
//                .HasForeignKey(x => x.AccreditationId);

//           e.HasMany(x => x.FileUploads)
//                .WithOne()
//                .HasForeignKey(x => x.AccreditationId);

//            e.HasMany(x => x.OverseasAccreditationSites)
//                .WithOne()
//                .HasForeignKey(x => x.AccreditationId);
//        });

//        modelBuilder.Entity<DataModels.Accreditations.AccreditationPrnIssueAuth>(e =>
//        {
//            e.ToTable("AccreditationPRNIssueAuth");
//        });

//        modelBuilder.Entity<AccreditationStatus>()
//            .HasData(
//                new AccreditationStatus { Id = 1, Name = "Started" },
//                new AccreditationStatus { Id = 2, Name = "Submitted" },
//                new AccreditationStatus { Id = 3, Name = "Accepted" },
//                new AccreditationStatus { Id = 4, Name = "Queried" },
//                new AccreditationStatus { Id = 5, Name = "Updated" },
//                new AccreditationStatus { Id = 6, Name = "Granted" },
//                new AccreditationStatus { Id = 7, Name = "Refused" },
//                new AccreditationStatus { Id = 8, Name = "Withdrawn" },
//                new AccreditationStatus { Id = 9, Name = "Suspended" },
//                new AccreditationStatus { Id = 10, Name = "Cancelled" }
//            );

//        modelBuilder.Entity<DataModels.Accreditations.ApplicationType>().HasData(
//            new DataModels.Accreditations.ApplicationType { Id = 1, Name = "Reprocessor" },
//            new DataModels.Accreditations.ApplicationType { Id = 2, Name = "Exporter" });

//        modelBuilder.Entity<FileUploadType>().HasData(
//            new DataModels.Accreditations.ApplicationType { Id = 1, Name = "SamplingAndInspectionPlan" },
//            new DataModels.Accreditations.ApplicationType { Id = 2, Name = "OverseasSiteEvidence" });

//        modelBuilder.Entity<FileUploadStatus>().HasData(
//            new DataModels.Accreditations.ApplicationType { Id = 1, Name = "Upload complete" },
//            new DataModels.Accreditations.ApplicationType { Id = 2, Name = "Upload failed" },
//            new DataModels.Accreditations.ApplicationType { Id = 3, Name = "File deleted" });

//        modelBuilder.Entity<SiteCheckStatus>().HasData(
//            new SiteCheckStatus { Id = 1, Status = "Not Started" },
//            new SiteCheckStatus { Id = 2, Status = "In Progress" },
//            new SiteCheckStatus { Id = 3, Status = "Completed" });

//        modelBuilder.Entity<MeetConditionsOfExport>().HasData(
//            new MeetConditionsOfExport { Id = 1, ConditionFulfilment = "Yes, they fulfil all the conditions. I do not want to upload evidence of broadly equivalent standards" },
//            new MeetConditionsOfExport { Id = 2, ConditionFulfilment = "Yes, they fulfil all the conditions. I also want to upload evidence of broadly equivalent standards" },
//            new MeetConditionsOfExport { Id = 3, ConditionFulfilment = "No, they do not fulfil all the conditions" });

//        // TEMP:
//        modelBuilder.Entity<LookupMaterial>().HasData(
//            new LookupMaterial { Id = 1, MaterialName = "Plastic", MaterialCode = "PL" },
//            new LookupMaterial { Id = 2, MaterialName = "Steel", MaterialCode = "GL" },
//            new LookupMaterial { Id = 3, MaterialName = "Aluminium", MaterialCode = "AL" });
//        modelBuilder.Entity<RegistrationMaterial>().HasData(
//            new RegistrationMaterial { Id = 1, MaterialId = 1 },
//            new RegistrationMaterial { Id = 2, MaterialId = 2 },
//            new RegistrationMaterial { Id = 3, MaterialId = 3 });
//    }

//    public virtual DbSet<DataModels.Accreditations.ApplicationType> ApplicationTypes { get; set; }
//    public virtual DbSet<AccreditationStatus> AccreditationStatuses { get; set; }

//    public virtual DbSet<AccreditationEntity> Accreditations { get; set; }
//    public virtual DbSet<DataModels.Accreditations.AccreditationPrnIssueAuth> AccreditationPrnIssueAuths { get; set; }
//    public virtual DbSet<DataModels.Accreditations.OverseasAccreditationSite> OverseasAccreditationSites { get; set; }

//    public virtual DbSet<DataModels.Accreditations.AccreditationFileUpload> AccreditationFileUploads { get; set; }
//}
