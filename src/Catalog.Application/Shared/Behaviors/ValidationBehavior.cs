using Catalog.Application.Shared.Results;
using FluentValidation;
using MediatR;
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        // If TResponse is a Result<T>
        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var error = Error.Validation(
                "VALIDATION_ERROR",
                string.Join("; ", failures.Select(f => f.ErrorMessage))
            );

            var resultType = typeof(TResponse);
            var failureMethod = resultType.GetMethod("Failure", new[] { typeof(Error) });

            return (TResponse)failureMethod!.Invoke(null, new object[] { error })!;
        }

        // If TResponse is a non-generic Result
        if (typeof(TResponse) == typeof(Result))
        {
            var error = Error.Validation(
                "VALIDATION_ERROR",
                string.Join("; ", failures.Select(f => f.ErrorMessage))
            );

            return (TResponse)(object)Result.Failure(error);
        }

        // Fallback: throw
        throw new ValidationException(failures);
    }
}


/**
using FluentValidation;
using MediatR;

namespace Catalog.Application.Shared.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    // IReadOnlyCollection
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}

**/