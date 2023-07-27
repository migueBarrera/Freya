namespace Models.Core.Common;

public class PaginationFilter
{
    const int minPageNumber = 1;
    const int maxPageSize = 50;

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string? SearchArgument { get; set; }

    public PaginationFilter()
    {
        this.PageNumber = 1;
        this.PageSize = PageModelConst.PageSizeDefault;
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber < minPageNumber
            ? minPageNumber
            : pageNumber;
        this.PageSize = pageSize > maxPageSize 
            ? maxPageSize 
            : pageSize;
    }

    public PaginationFilter(int pageNumber, int pageSize, string searchArgument)
    {
        this.PageNumber = pageNumber < minPageNumber
            ? minPageNumber
            : pageNumber;
        this.PageSize = pageSize > maxPageSize 
            ? maxPageSize 
            : pageSize;

        SearchArgument = searchArgument;
    }
}
