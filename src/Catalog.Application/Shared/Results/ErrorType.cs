namespace Catalog.Application.Shared.Results;
public enum ErrorType
{
    Validation,
    Domain,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Infrastructure
}
