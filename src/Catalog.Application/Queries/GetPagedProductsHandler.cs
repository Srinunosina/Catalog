using Catalog.Application.DTOs;
using Catalog.Application.interfaces;
using Catalog.Application.Shared.Results;
using MediatR;

namespace Catalog.Application.Products.Queries;

public sealed class GetPagedProductsHandler : IRequestHandler<GetPagedProductsQuery, Result<PagedResult<ProductDto>>>
{
    private readonly IProductRepository _repository;
    private readonly IProductReadRepository _readRepository;

    public GetPagedProductsHandler(IProductRepository repository, IProductReadRepository readRepository)
    {
        _repository = repository;
        _readRepository = readRepository;
    }

    public async Task<Result<PagedResult<ProductDto>>> Handle(
     GetPagedProductsQuery query,
     CancellationToken ct)
    {
        var (items, totalCount) = await _readRepository.GetPagedAsync(
            query.Page,
            query.PageSize,
            ct);

        var paged = PagedResult<ProductDto>.Create(
            items,
            query.Page,
            query.PageSize,
            totalCount);

        return Result<PagedResult<ProductDto>>.Success(paged);
    }

}
