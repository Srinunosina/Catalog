using Catalog.Application.DTOs;
using Catalog.Application.interfaces;
using Catalog.Application.Shared.Results;
using MediatR;

namespace Catalog.Application.Queries;
public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IProductReadRepository _repo;

    public GetProductsQueryHandler(IProductReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var products = await _repo.GetProductsAsync(ct);
        return Result<IEnumerable<ProductDto>>.Success(products);
    }
}