
namespace Catalog.Api.Middlewares;
public sealed class CorrelationLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ILogger<CorrelationLoggingMiddleware> logger)
    {
        var correlationId = context.TraceIdentifier;

        using (logger.BeginScope("CorrelationId={CorrelationId}, Application={Application}",
            correlationId,
            "Srinu Nosina Middleware Enricher"))
        {
            await next(context);
        }

        //// Alternatively, use JSON console logging for structured logs. it formats logs as JSON objects.
        //builder.Logging.AddJsonConsole(options =>
        //{
        //    options.IncludeScopes = true;
        //});

        //using (logger.BeginScope(new Dictionary<string, object>
        //{
        //    ["CorrelationId"] = correlationId,
        //    ["Application"] = "Srinu Nosina 01"
        //}))
        //{
        //    await _next(context);
        //}
    }
}
