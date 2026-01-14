using Azure.Core;
using Catalog.Api.Contracts;
using Catalog.Api.Results;
using Catalog.Application.DTOs;
using Catalog.Application.Products.Commands;
using Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Catalog.Api;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _mediator.Send(new GetProductsQuery());
        return result.ToResponse();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {       
        var result = await _mediator.Send(new CreateProductCommand(request.Sku, request.Name, request.Price));
        return result.ToResponse();
    }
}

