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
            new() { Id = 1, StatusName = EprnStatus.ACCEPTED.ToString(), StatusDescription = "Prn Accepted"},
            new() { Id = 2, StatusName = EprnStatus.REJECTED.ToString(), StatusDescription = "Prn Rejected"},
            new() { Id = 3, StatusName = EprnStatus.CANCELLED.ToString(), StatusDescription = "Prn Cancelled"},
            new() { Id = 4, StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString(), StatusDescription = "Prn Awaiting Acceptance"}
        ];

	}
    public enum EprnStatus
    {
        ACCEPTED  = 1,
        REJECTED,
        CANCELLED,
		AWAITINGACCEPTANCE
    }
}

