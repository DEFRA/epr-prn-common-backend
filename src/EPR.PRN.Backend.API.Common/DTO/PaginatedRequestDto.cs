using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.Dto
{
    [ExcludeFromCodeCoverage]
    public class PaginatedRequestDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public string? FilterBy { get; set; }

        public string? SortBy { get; set; }

        /// <summary>The first and last obligation year a search can ask for; the same range the waste-obligations API accepts.</summary>
        public const int MinimumAcceptableIntoYear = 2023;

        public const int MaximumAcceptableIntoYear = 2050;

        /// <summary>Only notes that can be accepted into this obligation year now. Out of range is a 400, enforced by PaginatedRequestDtoValidator, not a search.</summary>
        public int? AcceptableIntoYear { get; set; }
    }
}
