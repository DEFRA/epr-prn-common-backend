﻿using System.Diagnostics.CodeAnalysis;

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
    }
}
