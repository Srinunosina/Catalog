using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Shared.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> (
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse>
{
    private static readonly ActivitySource ActivitySource = new("Catalog.MediatR");

    private static readonly Meter Meter = new("Catalog.MediatR");

    private static readonly Histogram<double> DurationHistogram =
        Meter.CreateHistogram<double>(
            "mediatr.request.duration.ms",
            unit: "ms",
            description: "Duration of MediatR requests");

    private static readonly Counter<int> ErrorCounter =
        Meter.CreateCounter<int>(
            "mediatr.request.errors");

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        using var activity = ActivitySource.StartActivity(
            $"MediatR {requestName}",
            ActivityKind.Internal);

        activity?.SetTag("mediatr.request.name", requestName);
        activity?.SetTag("mediatr.request.type", typeof(TRequest).FullName);
        Stopwatch stopwatch = null;
        try
        {
            logger.LogInformation(
                "[START] {RequestName} TraceId={TraceId} SpanId={SpanId}",
                requestName,
                Activity.Current?.TraceId,
                Activity.Current?.SpanId
            );

            stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            DurationHistogram.Record(stopwatch.Elapsed.TotalMilliseconds);

            activity?.SetTag("mediatr.duration.ms", stopwatch.ElapsedMilliseconds);

            logger.LogInformation(
                "[END] {RequestName} Duration={Duration}ms TraceId={TraceId}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                Activity.Current?.TraceId
            );

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            ErrorCounter.Add(1);

            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.SetTag("error", true);
            activity?.SetTag("exception.type", ex.GetType().FullName);
            activity?.SetTag("exception.message", ex.Message);
            activity?.SetTag("exception.stacktrace", ex.StackTrace);

            logger.LogError(
                ex,
                "[ERROR] {RequestName} Duration={Duration}ms TraceId={TraceId}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                Activity.Current?.TraceId
            );

            throw;
        }
    }
}



//using MediatR;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;

//namespace Catalog.Application.Shared.Behaviors;

//public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//{
//    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

//    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
//    {
//        _logger = logger;
//    }

//    public async Task<TResponse> Handle(
//        TRequest request,
//        RequestHandlerDelegate<TResponse> next,
//        CancellationToken cancellationToken)
//    {
//        var requestName = typeof(TRequest).Name;
//        var traceId = Activity.Current?.TraceId.ToString();

//        _logger.LogInformation(
//            "Handling RequestName={RequestName} TraceId={TraceId} Payload={@Request}",
//            requestName,
//            traceId,
//            request
//        );

//        var response = await next();

//        _logger.LogInformation(
//           "Handling RequestName={RequestName} TraceId={TraceId} Payload={@Request}",
//            requestName,
//            traceId,
//            response
//        );

//        return response;
//    }
//}


