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

            modelBuilder.Entity<RecyclingTarget>()
                .HasData(
                            new RecyclingTarget { Year = 2025, PaperTarget = 0.75, GlassTarget = 0.74, AluminiumTarget = 0.61, SteelTarget = 0.8, PlasticTarget = 0.55, WoodTarget = 0.45, GlassRemeltTarget = 0.75 },
                            new RecyclingTarget { Year = 2026, PaperTarget = 0.77, GlassTarget = 0.76, AluminiumTarget = 0.62, SteelTarget = 0.81, PlasticTarget = 0.57, WoodTarget = 0.46, GlassRemeltTarget = 0.76 },
                            new RecyclingTarget { Year = 2027, PaperTarget = 0.79, GlassTarget = 0.78, AluminiumTarget = 0.63, SteelTarget = 0.82, PlasticTarget = 0.59, WoodTarget = 0.47, GlassRemeltTarget = 0.77 },
                            new RecyclingTarget { Year = 2028, PaperTarget = 0.81, GlassTarget = 0.80, AluminiumTarget = 0.64, SteelTarget = 0.83, PlasticTarget = 0.61, WoodTarget = 0.48, GlassRemeltTarget = 0.78 },
                            new RecyclingTarget { Year = 2029, PaperTarget = 0.83, GlassTarget = 0.82, AluminiumTarget = 0.65, SteelTarget = 0.84, PlasticTarget = 0.63, WoodTarget = 0.49, GlassRemeltTarget = 0.79 },
                            new RecyclingTarget { Year = 2030, PaperTarget = 0.85, GlassTarget = 0.85, AluminiumTarget = 0.67, SteelTarget = 0.85, PlasticTarget = 0.65, WoodTarget = 0.50, GlassRemeltTarget = 0.80 }
                        );

            modelBuilder.Entity<Material>()
                .HasData(
                            new Material { MaterialCode = "PL", MaterialName = "Plastic" },
                            new Material { MaterialCode = "WD", MaterialName = "Wood" },
                            new Material { MaterialCode = "AL", MaterialName = "Aluminium" },
                            new Material { MaterialCode = "ST", MaterialName = "Steel" },
                            new Material { MaterialCode = "PC", MaterialName = "Paper" },
                            new Material { MaterialCode = "GL", MaterialName = "Glass" }
                );

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Eprn> Prn { get; set; }

        public virtual DbSet<PrnStatus> PrnStatus { get; set; }

        public virtual DbSet<PrnStatusHistory> PrnStatusHistory { get; set; }

        public virtual DbSet<RecyclingTarget> RecyclingTargets { get; set; }

        public virtual DbSet<ObligationCalculation> ObligationCalculations { get; set; }

        public virtual DbSet<Material> Material { get; set; }

        public virtual DbSet<PEprNpwdSync> PEprNpwdSync { get; set; }
    }
}