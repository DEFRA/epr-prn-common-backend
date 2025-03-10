using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
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
					new Material { Id = 1, MaterialCode = "PL", MaterialName = MaterialType.Plastic.ToString(), IsCaculable = true, IsVisibleToObligation = true },
					new Material { Id = 2, MaterialCode = "WD", MaterialName = MaterialType.Wood.ToString(), IsCaculable = true, IsVisibleToObligation = true },
					new Material { Id = 3, MaterialCode = "AL", MaterialName = MaterialType.Aluminium.ToString(), IsCaculable = true, IsVisibleToObligation = true },
					new Material { Id = 4, MaterialCode = "ST", MaterialName = MaterialType.Steel.ToString(), IsCaculable = true, IsVisibleToObligation = true },
					new Material { Id = 5, MaterialCode = "PC", MaterialName = MaterialType.Paper.ToString(), IsCaculable = true, IsVisibleToObligation = true },
					new Material { Id = 6, MaterialCode = "GL", MaterialName = MaterialType.Glass.ToString(), IsCaculable = true, IsVisibleToObligation = true },
					new Material { Id = 7, MaterialCode = "GR", MaterialName = MaterialType.GlassRemelt.ToString(), IsCaculable = false, IsVisibleToObligation = true },
					new Material { Id = 8, MaterialCode = "FC", MaterialName = MaterialType.FibreComposite.ToString(), IsCaculable = true, IsVisibleToObligation = false }
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
    }
}