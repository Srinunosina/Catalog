using Catalog.Application.DTOs;
using Catalog.Application.interfaces;
using MediatR;

namespace Catalog.Application.Queries;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductReadRepository _repo;

    public GetProductsQueryHandler(IProductReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request,  CancellationToken ct)
    {
        return await _repo.GetProductsAsync(ct);
    }
}
