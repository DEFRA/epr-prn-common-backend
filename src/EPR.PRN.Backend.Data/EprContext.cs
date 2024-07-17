using EPR.PRN.Backend.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data
{
	public class EprContext : DbContext
	{
		public EprContext()
		{
		}

		public EprContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EPRN>()
				.HasIndex(a => a.ExternalId)
				.IsUnique();

			modelBuilder.Entity<EPRN>()
				.HasIndex(a => a.PrnNumber)
				.IsUnique();
			
			base.OnModelCreating(modelBuilder);
		}

		public virtual DbSet<EPRN> Prn { get; set; }
		
		public virtual DbSet<PrnStatus> PrnStatus { get; set; }

		public virtual DbSet<PrnStatusHistory> PrnStatusHistory { get; set; }
	}
}
