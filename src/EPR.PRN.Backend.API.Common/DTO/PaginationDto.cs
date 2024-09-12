using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.DTO
{
    [ExcludeFromCodeCoverage]
    public class PaginationDto
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
