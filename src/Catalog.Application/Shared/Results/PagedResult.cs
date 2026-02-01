namespace Catalog.Application.Shared.Results;

public sealed class PagedResult<T> : Result<IEnumerable<T>>, IPagedResult
{
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    object? IValueResult.Value => Items;

    public IEnumerable<T> Items { get; }

    private PagedResult(
        IEnumerable<T> items,
        int page,
        int pageSize,
        int totalCount)
        : base(true, items, null)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int page,
        int pageSize,
        int totalCount)
        => new(items, page, pageSize, totalCount);
}
