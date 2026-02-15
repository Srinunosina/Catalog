using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Catalog.Api.Extentions;
public static class ObservabilityExtensions
{
    public static WebApplicationBuilder AddObservability(this WebApplicationBuilder builder)
    {
        // 1. The Manual Bootstrap (Ensures ActivitySource.StartActivity is never null)
        ActivitySource.AddActivityListener(new ActivityListener
        {
            ShouldListenTo = source => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) =>
                ActivitySamplingResult.AllDataAndRecorded,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) =>
                ActivitySamplingResult.AllDataAndRecorded
        });

        // 2. The Production Engine (OpenTelemetry)
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService("Catalog.Api"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource("Catalog.MediatR")
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddConsoleExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddMeter("Catalog.MediatR")
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter();
            });

        return builder;
    }
}
