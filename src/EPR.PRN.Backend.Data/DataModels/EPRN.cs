﻿using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
	public class Eprn
	{
		[Key]
		public int Id { get; set; }
		
		[Required]
		public Guid ExternalId { get; set; }
		
		[MaxLength(20)]
		public required string PrnNumber { get; set; }
		
		[Required]
		public Guid OrganisationId { get; set; }
		
		[Required, MaxLength(160)]
		public required string OrganisationName { get; set; }
		
		[MaxLength(50)]
		public required string ProducerAgency { get; set; }
		
		[MaxLength(50)]
		public required string ReprocessorExporterAgency { get; set; }
		
		public int PrnStatusId { get; set; }
		
		public int TonnageValue { get; set; }
		
		[MaxLength(20)]
		public required string MaterialName { get; set; }
		
		[MaxLength(200)]
		public string? IssuerNotes { get; set; }
		
		[MaxLength(200)]
		public required string IssuerReference { get; set; }
		
		[MaxLength(50)]
		public string? PrnSignatory { get; set; }
		
		[MaxLength(50)]
		public string? PrnSignatoryPosition { get; set; }
		
		[MaxLength(100)]
		public string? Signature { get; set; }
		
		public DateTime IssueDate { get; set; }
		
		[MaxLength(20)]
		public string? ProcessToBeUsed { get; set; }
		
		public bool DecemberWaste { get; set; }
		
		public DateTime? StatusUpdatedOn { get; set; }
		
		[MaxLength(50)]
		public required string IssuedByOrg { get; set; }
		
		[MaxLength(20)]
		public required string AccreditationNumber { get; set; }
		
		[MaxLength(100)]
		public required string? ReprocessingSite { get; set; }
		
		[MaxLength(10)]
		public required string AccreditationYear { get; set; }
		
		[MaxLength(10)]
		public required string ObligationYear { get; set; }
		
		[MaxLength(100)]
		public required string PackagingProducer { get; set; }
		
		[MaxLength(20)]
		public string? CreatedBy { get; set; }
		
		public DateTime CreatedOn { get; set; }
		
		[Required]
		public Guid LastUpdatedBy { get; set; }
		
		public DateTime LastUpdatedDate { get; set; }
		
		public bool IsExport { get; set; }

		public virtual ICollection<PrnStatusHistory> PrnStatusHistories { get; set; } = null!;

    }
}