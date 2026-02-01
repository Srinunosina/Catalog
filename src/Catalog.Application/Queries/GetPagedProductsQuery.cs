using Catalog.Application.DTOs;
using Catalog.Application.Shared.Results;
using MediatR;

public record GetPagedProductsQuery(int Page, int PageSize)
    : IRequest<Result<PagedResult<ProductDto>>>;
