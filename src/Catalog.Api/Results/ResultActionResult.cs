using Catalog.Application.Shared.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Catalog.Api.Results;

public sealed class ResultActionResult : IActionResult
{
    private readonly ResultBase _result;

    public ResultActionResult(ResultBase result)
    {
        _result = result;
    }

    
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var http = context.HttpContext;

        // -----------------------------
        // SUCCESS
        // -----------------------------
        if (_result.IsSuccess)
        {
            // 204 No Content (Result without value)
            if (_result is Result r && r.Error is null)
            {
                await new NoContentResult().ExecuteResultAsync(context);
                return;
            }

            // 201 Created (CreatedResult<T>)
            if (_result is ICreatedResult created)
            {
                http.Response.Headers.Location = created.Location;

                var objectResult = new ObjectResult(created.Value)
                {
                    StatusCode = StatusCodes.Status201Created
                };

                await objectResult.ExecuteResultAsync(context);
                return;
            }

            // 200 OK (Result<T>)
            if (_result is IValueResult valueResult)
            {
                await new OkObjectResult(valueResult.Value).ExecuteResultAsync(context);
                return;
            }

            //Paged results
            if (_result is IPagedResult paged)
            {
                var response = new
                {
                    data = paged.Value,
                    page = paged.Page,
                    pageSize = paged.PageSize,
                    totalCount = paged.TotalCount,
                    totalPages = paged.TotalPages
                };

                await new OkObjectResult(response).ExecuteResultAsync(context);
                return;
            }


            // Fallback: NoContent
            await new NoContentResult().ExecuteResultAsync(context);
            return;
        }

        // -----------------------------
        // FAILURE → ProblemDetails
        // -----------------------------
        var problemDetailsService =
            http.RequestServices.GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = http,
            ProblemDetails = new ProblemDetails
            {
                Title = _result.Error!.Code,
                Detail = _result.Error.Message,
                Status = MapStatusCode(_result.Error.Type)
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
            ErrorType.Infrastructure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
}

/**
 What ResultActionResult Already Guarantees (Very Important)

Your ResultActionResult already enforces:

✅ Success semantics (200 / 201 / 204)

✅ Paged responses

✅ Failure → ProblemDetails

✅ Centralized HTTP mapping

✅ RFC-7807 compliance

✅ Deterministic contract

This class is effectively:

The single exit gate of the HTTP layer

That means:

Controllers

MediatR

Filters

…must all funnel into this.
**/



/**
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

            if (_result is CreatedResult<object> created)
            {
                var objectResult = new ObjectResult(created.Value)
                {
                    StatusCode = StatusCodes.Status201Created
                };

                httpContext.Response.Headers.Location = created.Location;

                await objectResult.ExecuteResultAsync(httpContext);
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
**/