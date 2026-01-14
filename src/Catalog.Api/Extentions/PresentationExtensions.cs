using Catalog.Api.Middleware;
using Microsoft.OpenApi;

namespace Catalog.Api.Extentions;

public static class PresentationExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Catalog API",
                Version = "v1",
                Description = "High-performance Catalog microservice with clean architecture."
            });
        });

        services.AddTransient<GlobalExceptionMiddleware>();

        return services;
    }

    public static IApplicationBuilder UsePresentation(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        // Developer exception page
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
            options.RoutePrefix = string.Empty;
        });

        // Routing
        app.MapControllers();

        return app;
    }

    /**
     UI Layer → Middlewares
    ----------------------
    1.GlobalExceptionMiddleware
    2.Authentication
    3.Authorization
    4.CORS
    5.Routing
    6.Swagger
    **/

}


