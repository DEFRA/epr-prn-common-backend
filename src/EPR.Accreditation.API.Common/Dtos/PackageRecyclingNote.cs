using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class PackageRecyclingNote
    {
        public Guid? ExternalId { get; set; } // This has a unique key added via the dbcontext

        public Enums.OperatorType OperatorTypeId { get; set; }

        [MaxLength(12)]
        public string ReferenceNumber { get; set; } // This has a unique key added via the dbcontext

        public Guid OrganisationId { get; set; }

    }
}