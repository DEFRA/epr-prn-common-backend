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

		public static readonly List<PrnStatus> Data =
        [
            new() { Id = 1, StatusName = PrnStatusEnum.ACCEPTED.ToString(), StatusDescription = "Prn Accepted"},
            new() { Id = 2, StatusName = PrnStatusEnum.REJECTED.ToString(), StatusDescription = "Prn Rejected"},
            new() { Id = 3, StatusName = PrnStatusEnum.CANCELED.ToString(), StatusDescription = "Prn Cancelled"},
            new() { Id = 4, StatusName = PrnStatusEnum.AWAITINGACCEPTANCE.ToString(), StatusDescription = "Prn Awaiting Acceptance"}
        ];

	}
    public enum PrnStatusEnum
    {
        ACCEPTED  = 1,
        REJECTED,
        CANCELED,
		AWAITINGACCEPTANCE
    }
}

