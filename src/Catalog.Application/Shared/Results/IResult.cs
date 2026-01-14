namespace Catalog.Application.Shared.Results;

public interface IResult
{
    bool IsSuccess { get; }
    Error? Error { get; }
}

