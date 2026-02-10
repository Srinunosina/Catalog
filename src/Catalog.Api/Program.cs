using Catalog.Api.Extentions;
using Catalog.Application.Extensions;
using Catalog.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = true;
});

builder.Logging.AddDebug();

builder.Services
    .AddPresentation()     // Controllers, Swagger, Middleware
    .AddApplication()      // MediatR, Behaviors, IResult, Validators
    .AddInfrastructure(builder.Configuration); // DbContext, Repos, Mapping

var app = builder.Build();
app.UsePresentation();     // Middleware pipeline

app.Run();