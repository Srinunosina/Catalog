namespace Catalog.Infrastructure.Logging;

//public sealed class DatabaseLogSink : ILogSink
//{
//    private readonly LogDbContext _db;

//    public DatabaseLogSink(LogDbContext db)
//    {
//        _db = db;
//    }

//    public void Write(LogEntry entry)
//    {
//        _db.Logs.Add(new LogEntity
//        {
//            Timestamp = entry.Timestamp,
//            Level = entry.Level,
//            Category = entry.Category,
//            Message = entry.Message,
//            Exception = entry.Exception,
//            CorrelationId = entry.CorrelationId,
//            Application = entry.Application
//        });

//        _db.SaveChanges();
//    }
//}
