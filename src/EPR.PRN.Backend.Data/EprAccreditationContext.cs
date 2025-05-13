using System.Data;
using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data;

[ExcludeFromCodeCoverage]
public class EprAccreditationContext : DbContext
{
    public EprAccreditationContext()
    {
    }

    public EprAccreditationContext(DbContextOptions<EprAccreditationContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("EprAccreditationDatabase");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accreditation>(e =>
        {
            e.HasOne(x => x.ApplicationType)
                .WithMany()
                .HasForeignKey(x => x.ApplicationTypeId);

            e.HasOne(x => x.AccreditationStatus)
                .WithMany()
                .HasForeignKey(x => x.AccreditationStatusId);

            e.HasOne(x => x.RegistrationMaterial)
                .WithMany()
                .HasForeignKey(x => x.RegistrationMaterialId);

            e.HasMany(x => x.AccreditationPrnIssueAuths)
                .WithOne()
                .HasForeignKey(x => x.AccreditationId);
        });

        modelBuilder.Entity<AccreditationPrnIssueAuth>(e =>
        {
            e.ToTable("AccreditationPRNIssueAuth");
        });

        modelBuilder.Entity<AccreditationStatus>()
            .HasData(
                new AccreditationStatus { Id = 1, Name = "Started" },
                new AccreditationStatus { Id = 2, Name = "Submitted" },
                new AccreditationStatus { Id = 3, Name = "Accepted" },
                new AccreditationStatus { Id = 4, Name = "Queried" },
                new AccreditationStatus { Id = 5, Name = "Updated" },
                new AccreditationStatus { Id = 6, Name = "Granted" },
                new AccreditationStatus { Id = 7, Name = "Refused" },
                new AccreditationStatus { Id = 8, Name = "Withdrawn" },
                new AccreditationStatus { Id = 9, Name = "Suspended" },
                new AccreditationStatus { Id = 10, Name = "Cancelled" }
            );

        modelBuilder.Entity<ApplicationType>().HasData(
            new ApplicationType { Id = 1, Name = "Reprocessor" },
            new ApplicationType { Id = 2, Name = "Exporter" });

        // TEMP:
        modelBuilder.Entity<LookupMaterial>().HasData(
            new LookupMaterial { Id = 1, MaterialName = "Plastic", MaterialCode = "PL" },
            new LookupMaterial { Id = 2, MaterialName = "Steel", MaterialCode = "GL" },
            new LookupMaterial { Id = 3, MaterialName = "Aluminium", MaterialCode = "AL" });
        modelBuilder.Entity<RegistrationMaterial>().HasData(
            new RegistrationMaterial { Id = 1, MaterialId = 1 },
            new RegistrationMaterial { Id = 2, MaterialId = 2 },
            new RegistrationMaterial { Id = 3, MaterialId = 3 });
    }

    public DbSet<ApplicationType> ApplicationTypes { get; set; }
    public DbSet<AccreditationStatus> AccreditationStatuses { get; set; }
    public DbSet<Accreditation> Accreditations { get; set; }
    public DbSet<AccreditationPrnIssueAuth> AccreditationPrnIssueAuths { get; set; }
}

