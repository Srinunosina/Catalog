namespace Catalog.Application.Shared.Results;
public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new("", "", default);

    public static Error Validation(string code, string message)
        => new(code, message, ErrorType.Validation);

    public static Error Domain(string code, string message)
        => new(code, message, ErrorType.Domain);

    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message)
        => new(code, message, ErrorType.Conflict);

    public static Error Unauthorized(string code, string message)
        => new(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string message)
        => new(code, message, ErrorType.Forbidden);

    public static Error Infrastructure(string code, string message)
        => new(code, message, ErrorType.Infrastructure);
}


/**
namespace Catalog.Application.Shared.Results;

public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new("", "", default);
}
**/

