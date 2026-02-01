using Catalog.Api.Contracts;
using Catalog.Application.Products.Commands;
using Catalog.Application.Queries;
using Catalog.Infrastructure.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers.Api;

[ApiController]
[Route("api/products")]
public class ProductsController(
    IMediator mediator,
    ILogger<ProductsController> logger,
    IAppLogger<ProductsController> appLogger
    ) : ControllerBase
{

    [HttpGet("/test-log")]
    public IActionResult Test()
    {
        appLogger.Info("Order processing started");

        //try
        //{
            throw new InvalidOperationException("Boom");
        //}
        //catch (Exception ex)
        //{
        //    appLogger.Error(ex, "Order processing failed");
        //}

        //return Ok("Logged to all sinks");
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await mediator.Send(new GetProductsQuery());
        return result.ToResponse();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var result = await mediator.Send(new CreateProductCommand(request.Sku, request.Name, request.Price));
        return result.ToResponse();
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        var result = await mediator.Send(new GetPagedProductsQuery(page, size));
        return result.ToResponse();
    }
}

