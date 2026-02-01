using Catalog.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Catalog.Api.Middleware;
public sealed class GlobalExceptionMiddleware (
    IProblemDetailsService problemDetails,
    ILogger<GlobalExceptionMiddleware> logger

    ) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            logger.LogError(ex,
                "Unhandled exception | TraceId: {TraceId} | Path: {Path}",
                traceId,
                context.Request.Path);

            var env = context.RequestServices.GetRequiredService<IHostEnvironment>();

            await problemDetails.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
            {
                Title = "Internal Server Error",
                Detail = env.IsDevelopment()
                    ? ex.Message
                    : "An unexpected error occurred. Please contact support.",
                Status = StatusCodes.Status500InternalServerError,
                Instance = context.Request.Path,
                Extensions =
                {
                    ["traceId"] = traceId
                }
            }
            });
        }
    }
}

// Summary of Exception Handling Flow
/**
 
| Origin             | Translation                             | Normalization        |
| ------------------ | --------------------------------------- | -------------------- |
| MediatR            | `ExceptionHandlingBehavior`             | `ResultActionResult` |
| MVC(no MediatR)   | `GlobalExceptionFilter`                 | `ResultActionResult` |
| Controller success | `return new ResultActionResult(result)` | `ResultActionResult` |
| Uncaught           | GlobalExceptionMiddleware               | `ProblemDetails`     |

**/