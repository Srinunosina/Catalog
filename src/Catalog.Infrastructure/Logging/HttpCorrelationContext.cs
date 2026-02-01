using Microsoft.AspNetCore.Http;

namespace Catalog.Infrastructure.Logging;

public sealed class HttpCorrelationContext : ICorrelationContext
{
    public string CorrelationId { get; }

    public HttpCorrelationContext(IHttpContextAccessor accessor)
    {
        CorrelationId =
            accessor.HttpContext?.TraceIdentifier
            ?? Guid.NewGuid().ToString();
    }
}
