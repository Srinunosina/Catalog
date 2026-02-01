namespace Catalog.Application.Shared.Results;
public sealed class CreatedResult<T> : Result<T>, ICreatedResult
{
    public string Location { get; }
    object? IValueResult.Value => Value;
    private CreatedResult(T value, string location) : base(true, value, null)
    {
        Location = location;
    }
    public static CreatedResult<T> Create(T value, string location) => new(value, location);
}
