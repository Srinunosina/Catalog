/**
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Catalog.Application.Shared;
public static class ResultFactory
{
    private static readonly ConcurrentDictionary<Type, Func<Error, object>> _failureFactories = new();

    public static object CreateGenericFailure(Type resultType, Error error)
    {
        // resultType is like typeof(Result<Foo>)
        var factory = _failureFactories.GetOrAdd(resultType, CreateFactory);
        return factory(error);
    }

    private static Func<Error, object> CreateFactory(Type resultType)
    {
        // Validate type
        if (!resultType.IsGenericType || resultType.GetGenericTypeDefinition() != typeof(Result<>))
        {
            throw new InvalidOperationException(
                $"ResultFactory can only handle Result<T> types. Got: {resultType.FullName}");
        }

        // Locate static Failure(Error) method
        var methodInfo = resultType.GetMethod(
            nameof(Result<object>.Failure),
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            types: new[] { typeof(Error) },
            modifiers: null);

        if (methodInfo is null)
        {
            throw new InvalidOperationException(
                $"Could not find static Failure(Error) on {resultType.FullName}");
        }

        // Build a delegate: Error -> object
        var errorParam = Expression.Parameter(typeof(Error), "error");

        var call = Expression.Call(
            methodInfo,
            errorParam);

        var castToObject = Expression.Convert(call, typeof(object));

        var lambda = Expression.Lambda<Func<Error, object>>(
            castToObject,
            errorParam);

        return lambda.Compile();
    }
}

**/