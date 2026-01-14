using Catalog.Application.DTOs;
using Catalog.Application.Shared.Results;
using MediatR;
namespace Catalog.Application.Queries;
public record GetProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>;


