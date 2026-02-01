using Catalog.Api.Results;
using Catalog.Application.Shared.Results;
using Catalog.Domain.Shared;
using Catalog.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Catalog.Api.ActionFilter;
public sealed class GlobalExceptionFilter (
    IAppLogger<GlobalExceptionFilter> customApplogger,
    ILogger<GlobalExceptionFilter> logger
    ) : IExceptionFilter
{
    //private readonly ILogger<GlobalExceptionFilter> logger;

    //public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    //{
    //    this.logger = logger;
    //}

    public void OnException(ExceptionContext context)
    {
        var action = context.ActionDescriptor.DisplayName;
        var route = context.HttpContext.Request.Path;
        var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

        Error error;

        // This filter only handles requests outside MediatR
        if (context.Exception is DomainException)
        {
            error = new Error(
                Code: "DOMAIN_ERROR",
                Message: "A business rule was violated.",
                Type: ErrorType.Domain);
        }
        else
        {
            error = new Error(
                Code: "UNHANDLED_ERROR",
                Message: "Unhandled exception outside application pipeline.",
                Type: ErrorType.Infrastructure);
        }

        logger.LogError(
            context.Exception,
            "Unhandled exception | Action: {Action} | Route: {Route} | TraceId: {TraceId}",
            action,
            route,
            traceId);

        customApplogger.Error(context.Exception, "");
        var result = Result.Failure(error);

        // Funnel into the SAME result pipeline used everywhere else
        context.Result = new ResultActionResult(result);

        context.ExceptionHandled = true;
    }
}
