using Catalog.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Results;

public sealed class ResultActionResult<T> : IActionResult, Microsoft.AspNetCore.Http.IResult
{
    private readonly Result<T> _result;

    public ResultActionResult(Result<T> result)
    {
        _result = result;
    }

    // Entry point for MVC Controllers
    public async Task ExecuteResultAsync(ActionContext context)
        => await ExecuteAsync(context.HttpContext);

    // Entry point for Minimal APIs
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        if (_result.IsSuccess)
        {
            if (_result.Value is null || typeof(T) == typeof(object))
            {
                httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }

            await httpContext.Response.WriteAsJsonAsync(_result.Value);
            return;
        }

        // Deterministic Error Handling using ProblemDetails
        var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
        var statusCode = MapStatusCode(_result.Error!.Type);

        httpContext.Response.StatusCode = statusCode;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Type = $"https://httpstatuses.io/{statusCode}",
                Title = _result.Error.Code,
                Detail = _result.Error.Message,
                Status = statusCode,
                Instance = httpContext.Request.Path
            }
        });
    }

    private static int MapStatusCode(ErrorType type) =>
        type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Domain => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
}