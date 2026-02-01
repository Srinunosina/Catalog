namespace Catalog.Application.Shared.Results;
public interface ICreatedResult : IValueResult
{
    string Location { get; }
}
