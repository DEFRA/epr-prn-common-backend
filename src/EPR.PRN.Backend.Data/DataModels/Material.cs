using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Material
	{
		[Key]
		public int Id { get; set; }

        [MaxLength(20)]
        public required string MaterialName { get; set; }

		[MaxLength(3)]
        public required string MaterialCode { get; set; }

		public required bool IsCaculable { get; set; }

		public required bool IsVisibleToObligation { get; set; }

		public virtual ICollection<PrnMaterialMapping> PrnMaterialMappings { get; set; } = null!;
	}
}
