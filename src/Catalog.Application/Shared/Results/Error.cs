namespace Catalog.Application.Shared.Results;

public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new("", "", default);
}

