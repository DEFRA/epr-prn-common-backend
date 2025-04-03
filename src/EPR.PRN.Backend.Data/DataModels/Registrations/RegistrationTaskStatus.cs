using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class RegistrationTaskStatus
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public int TaskId { get; set; }
        public int RegistrationId { get; set; }
        public int StatusId { get; set; }
        public Guid StatusCreatedBy { get; set; }
        public DateTime StatusCreatedDate { get; set; }
        public Guid? StatusUpdatedBy { get; set; }
        public DateTime? StatusUpdatedDate { get; set; }
    }
}
