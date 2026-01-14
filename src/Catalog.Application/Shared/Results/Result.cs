namespace Catalog.Application.Shared.Results;
public sealed class Result : ResultBase
{
    private Result(bool isSuccess, Error? error) : base(isSuccess, error) { }
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}