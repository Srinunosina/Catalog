using Catalog.Infrastructure.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers.Api;

[ApiController]
[Route("api/products")]
public class OrderController(
    IMediator mediator,
    ILogger<OrderController> logger,
    IAppLogger<OrderController> appLogger
    ) : ControllerBase
{

    
}

