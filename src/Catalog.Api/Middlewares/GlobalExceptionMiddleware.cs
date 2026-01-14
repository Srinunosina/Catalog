namespace Catalog.Api.Middleware;
public sealed class GlobalExceptionMiddleware : IMiddleware
{
    private readonly IProblemDetailsService _problemDetails;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        IProblemDetailsService problemDetails,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _problemDetails = problemDetails;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await _problemDetails.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path
                }
            });
        }
    }
}


