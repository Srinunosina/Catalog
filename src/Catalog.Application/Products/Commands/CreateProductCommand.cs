using Catalog.Application.Shared.Results;
using MediatR;

namespace Catalog.Application.Products.Commands;
public sealed record CreateProductCommand(string Sku, string Name, decimal Price) : IRequest<Result<Guid>>;