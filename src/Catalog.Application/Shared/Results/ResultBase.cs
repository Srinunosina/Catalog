namespace Catalog.Application.Shared.Results;

public abstract class ResultBase : IResult
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected ResultBase(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
}

