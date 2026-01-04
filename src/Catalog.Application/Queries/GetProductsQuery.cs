using Catalog.Application.DTOs;
using MediatR;
namespace Catalog.Application.Queries;
public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;


