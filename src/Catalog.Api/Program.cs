using Catalog.Application.interfaces;
using Catalog.Application.Queries;
using Catalog.Infrastructure.Mapping;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "High-performance Catalog microservice with clean architecture."
    });
});

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).Assembly));

// Repositories
builder.Services.AddTransient<IProductReadRepository, ProductReadRepository>();

// DbContext
builder.Services.AddDbContext<CatalogDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Mapster
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(ProductMappingConfig).Assembly);
builder.Services.AddSingleton(config);

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

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

// Optional: HTTPS
// app.UseHttpsRedirection();

app.Run();
