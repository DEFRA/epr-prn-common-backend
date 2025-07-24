using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    // Currently this recrods fromd this table are deleted from cascade delete, feature not implemented for adding records.
    [ExcludeFromCodeCoverage]
    [Table("Public.InterimOverseasConnections")]
    public class InterimOverseasConnections
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }

        public int InterimSiteId { get; set; }
        [ForeignKey(nameof(InterimSiteId))]
        public OverseasAddress OverseasAddress { get; set; } = null!;

        public int ParentOverseasAddressId { get; set; }
        [ForeignKey(nameof(ParentOverseasAddressId))]
        public OverseasAddress ParentOverseasAddress { get; set; } = null!;
    }
}
