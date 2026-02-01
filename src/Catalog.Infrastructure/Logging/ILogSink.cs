namespace Catalog.Infrastructure.Logging;
/// <summary>
/// Delivery abstraction
/// </summary>
public interface ILogSink
{
    void Write(LogEntry entry);
}
