//using Catalog.Api.Contracts;
//using Catalog.Application.Products.Commands;
//using Catalog.Application.Queries;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Catalog.Api;

//[ApiController]
//[Route("api/products")]
//public class TestProductsController (
//    IMediator mediator,
//    ILogger<TestProductsController> logger
//    ) : ControllerBase
//{

//    [HttpGet]
//    public async Task<IActionResult> GetProducts()
//    {
//        var result = await mediator.Send(new GetProductsQuery());
//        return result.ToResponse();
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create(CreateProductRequest request)
//    {       
//        var result = await mediator.Send(new CreateProductCommand(request.Sku, request.Name, request.Price));
//        return result.ToResponse();
//    }

//    [HttpGet("paged")]
//    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 20)
//    {
//        var result = await mediator.Send(new GetPagedProductsQuery(page, size));
//        return result.ToResponse();
//    }
//}

