using Catalog.Application.DTOs;
namespace Catalog.Application.interfaces;
public interface IProductReadRepository
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken ct);  
    Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken ct);
}

