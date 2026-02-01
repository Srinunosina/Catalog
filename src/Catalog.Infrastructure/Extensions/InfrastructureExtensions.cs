using Catalog.Application.interfaces;
using Catalog.Infrastructure.Logging;
using Catalog.Infrastructure.Mapping;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration config)
    {
        services.AddDbContext<CatalogDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("Default")));

        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(typeof(ProductMappingConfig).Assembly);
        services.AddSingleton(mapsterConfig);

        services.AddTransient<IProductReadRepository, ProductReadRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();


       // services.AddHttpContextAccessor();

        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        services.AddScoped<ICorrelationContext, HttpCorrelationContext>();

        // Enable / disable sinks here
        services.AddScoped<ILogSink, ConsoleLogSink>();
        services.AddScoped<ILogSink, FileLogSink>();
        //   services.AddScoped<ILogSink, DatabaseLogSink>();
        //  services.AddDbContext<LogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Logs")));


        return services;
    }
}


