namespace Catalog.Infrastructure.Logging;

public sealed class FileLogSink : ILogSink
{
    private const string FilePath = "logs/app.log";

    public void Write(LogEntry entry)
    {
        Directory.CreateDirectory("logs");

        var line =
            $"{entry.Timestamp:o}|{entry.Level}|{entry.Category}|{entry.CorrelationId}|{entry.Message}{Environment.NewLine}";

        File.AppendAllText(FilePath, line);
    }
}
