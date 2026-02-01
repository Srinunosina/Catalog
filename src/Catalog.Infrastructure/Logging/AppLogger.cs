namespace Catalog.Infrastructure.Logging;
/// <summary>
/// The Orchestrator, 💡 No if statements 💡 No sink knowledge 💡 Pure fan-out
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="sinks"></param>
/// <param name="correlation"></param>
public sealed class AppLogger<T> (
    IEnumerable<ILogSink> sinks,
    ICorrelationContext correlation

    ) : IAppLogger<T>
{
    public void Info(string message) => Write("INFO", message, null);

    public void Error(Exception ex, string message) => Write("ERROR", message, ex);

    private void Write(string level, string message, Exception? ex)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            Category = typeof(T).FullName!,
            Message = message,
            Exception = ex?.ToString(),
            CorrelationId = correlation.CorrelationId,
            Application = "Srinu Nosina"
        };

        foreach (var sink in sinks)
        {
            sink.Write(entry);
        }
    }
}
