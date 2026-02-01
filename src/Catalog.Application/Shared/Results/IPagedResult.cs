namespace Catalog.Application.Shared.Results;

public interface IPagedResult : IValueResult
{
    int Page { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
}
