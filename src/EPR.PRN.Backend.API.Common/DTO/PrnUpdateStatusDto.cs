using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Common.DTO
{
    public class PrnUpdateStatusDto
    {
        public Guid PrnId { get; set; }

        public required PrnStatusEnum Status { get; set; }
    }
}
