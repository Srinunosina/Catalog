namespace Catalog.Infrastructure.Logging;

/// <summary>
/// Plug & Play
/// </summary>
public sealed class ConsoleLogSink : ILogSink
{
    public void Write(LogEntry entry)
    {
        Console.WriteLine(
            $"{entry.Timestamp:o} [{entry.Level}] [{entry.Application}] [{entry.CorrelationId}] {entry.Category} - {entry.Message}");
    }
}
