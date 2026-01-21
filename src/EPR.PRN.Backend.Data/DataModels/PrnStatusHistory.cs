using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
	public class PrnStatusHistory
	{
		[Key]
		public int Id { get; set; }
		
		public DateTime CreatedOn { get; set; }
		
		[Required]
		public Guid CreatedByUser { get; set; }
		
		[Required]
		public Guid CreatedByOrganisationId { get; set; }
		
		public int PrnStatusIdFk { get; set; }
		
		public int PrnIdFk { get; set; }
		
		[MaxLength(1000)]
		public string? Comment { get; set; }

		[MaxLength(10)]
		public string? ObligationYear { get; set; }
	}
}
