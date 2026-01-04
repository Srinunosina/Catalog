using Catalog.Application.DTOs;
namespace Catalog.Application.interfaces;
public interface IProductReadRepository
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken ct);
}

