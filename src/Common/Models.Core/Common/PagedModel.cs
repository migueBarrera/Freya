using System.Text.Json.Serialization;

namespace Models.Core.Common;

public class PagedModel<T>
{
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public string? SearchArgument { get; set; }

    public List<T> Items { get; set; } = new List<T>();

    [JsonConstructor]
    public PagedModel() { }

    public PagedModel(List<T> items, int count, int currentPage, int pageSize, string? searchArgument = null)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        SearchArgument = searchArgument;
        Items.AddRange(items);
    }

    public static PagedModel<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, string? searchArgument = null)
    {
        var count = source.Count();
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedModel<T>(items, count, pageNumber, pageSize, searchArgument);
    }
    
    public static PagedModel<T> Empty()
    {
        var count = 0;
        var items = Enumerable.Empty<T>().ToList();
        return new PagedModel<T>(items, count, 1, PageModelConst.PageSizeDefault, string.Empty);
    }

    public static PagedModel<T> EmptyFrom<Y>(PagedModel<Y> pageList)
    {
        var items = Enumerable.Empty<T>().ToList();
        return new PagedModel<T>(items, pageList.TotalCount, pageList.CurrentPage, pageList.PageSize, pageList.SearchArgument);
    }
}
