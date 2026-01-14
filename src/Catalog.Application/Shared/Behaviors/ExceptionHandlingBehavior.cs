using Catalog.Application.Shared.Results;
using Catalog.Domain.Shared;
using MediatR;

namespace Catalog.Application.Shared.Behaviors;

public sealed class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultBase
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DomainException ex)
        {
            var error = new Error("DOMAIN_ERROR", ex.Message, ErrorType.Domain);
            return CreateFailureResponse(error);
        }
        catch (Exception ex)
        {
            var error = new Error("INFRA_ERROR", $"Unexpected system failure: {ex.Message}", ErrorType.Infrastructure);
            return CreateFailureResponse(error);
        }
    }

    private static TResponse CreateFailureResponse(Error error)
    {
        var responseType = typeof(TResponse);

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            return (TResponse)ResultFactory.CreateGenericFailure(responseType, error);
        }

        // Non-generic Result
        return (TResponse)(object)Result.Failure(error);
    }
}
