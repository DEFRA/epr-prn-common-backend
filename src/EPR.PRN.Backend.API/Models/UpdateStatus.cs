#nullable disable
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Models
{
    [ExcludeFromCodeCoverage]
    public class UpdateStatus
    {
        public List<PrnUpdateStatusDto> UpdateStatusList { get; set; }
    }
}