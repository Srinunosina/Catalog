using Catalog.Api.Results;
using Catalog.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;
public static class ResultExtensions
{
    public static IActionResult ToResponse(this ResultBase result) => new ResultActionResult(result);
}

/**
using Catalog.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Results;
public static class ResultExtensions
{
    // Handles Result<T>
    public static ResultActionResult<T> ToResponse<T>(this Result<T> result) => new(result);

    // Handles Result (Non-generic) by converting to a Result<object>
    public static ResultActionResult<object> ToResponse(this Result result)
    {
        var genericResult = result.IsSuccess
            ? Result<object>.Success(null!)
            : Result<object>.Failure(result.Error!);

        return new ResultActionResult<object>(genericResult);
    }
}
**/
