namespace Catalog.Infrastructure.Logging;

public interface ICorrelationContext
{
    string CorrelationId { get; }
}
