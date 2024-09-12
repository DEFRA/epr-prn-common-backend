using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.DTO
{
    [ExcludeFromCodeCoverage]
    public class PaginatedResponseDto<T>
    {
	    public List<T> Items { get; set; }

	    public int CurrentPage { get; set; }

	    public int TotalItems { get; set; }

	    public int PageSize { get; set; }
		
		public string? SearchTerm { get; set; }

		public string? FilterBy { get; set; }
		
		public string? SortBy { get; set; }		

		public int PageCount
	    {
		    get
		    {
			    if (PageSize == 0)
			    {
				    return 0;
			    }

			    return (TotalItems + (PageSize - 1)) / PageSize;
		    }
	    }
    }
}
