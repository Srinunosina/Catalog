using Catalog.Application.Shared.Results;

namespace Catalog.Api.Results;
public static class MinimalApiResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToMinimalResponse(this Catalog.Application.Shared.Results.IResult result)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.NoContent()
            : MapMinimalError(result.Error);
    }

    public static Microsoft.AspNetCore.Http.IResult ToMinimalResponse<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? Microsoft.AspNetCore.Http.Results.Ok(result.Value)
            : MapMinimalError(result.Error);
    }

    private static Microsoft.AspNetCore.Http.IResult MapMinimalError(Error? error)
    {
        return error?.Type switch
        {
            ErrorType.NotFound => Microsoft.AspNetCore.Http.Results.NotFound(error),
            ErrorType.Validation => Microsoft.AspNetCore.Http.Results.BadRequest(error),
            _ => Microsoft.AspNetCore.Http.Results.Problem(error?.Code)
        };
    }
}