using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
	public class PrnStatus
	{
		[Key]
		public int Id { get; set; }
		
		[Required, MaxLength(20)]
		public required string StatusName { get; set; }
		
		[MaxLength(50)]
		public string? StatusDescription { get; set; }
	}
}

