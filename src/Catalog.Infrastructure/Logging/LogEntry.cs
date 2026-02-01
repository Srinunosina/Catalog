namespace Catalog.Infrastructure.Logging;

/// <summary>
/// ✅ Immutable ✅ Serializable ✅ Same object goes to all sinks
/// </summary>
public sealed class LogEntry
{
    public DateTime Timestamp { get; init; }
    public string Level { get; init; } = default!;
    public string Category { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string? Exception { get; init; }

    public string CorrelationId { get; init; } = default!;
    public string Application { get; init; } = default!;
}
