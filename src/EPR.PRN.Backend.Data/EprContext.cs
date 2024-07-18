using EPR.PRN.Backend.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EPR.PRN.Backend.Data
{
	public class EprContext : DbContext
	{
		private readonly IConfiguration _configuration;
		
		public EprContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		public EprContext(DbContextOptions options, IConfiguration configuration) : base(options)
		{
			_configuration = configuration;
		}
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_configuration.GetConnectionString("EprnConnectionString"));
			}
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
