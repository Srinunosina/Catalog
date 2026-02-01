namespace Catalog.Infrastructure.Logging;

/// <summary>
/// out T = covariance
/// Logger does not consume T, only categorizes
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAppLogger<out T>
{
    void Info(string message);
    void Error(Exception ex, string message);
}
