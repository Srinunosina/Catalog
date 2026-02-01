using Catalog.Application.DTOs;
using Catalog.Application.interfaces;
using Catalog.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace Catalog.Infrastructure.Repositories;

public class ProductReadRepository : IProductReadRepository
{
    private readonly CatalogDbContext _dbContext;
    public ProductReadRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken ct)
    {
        var products = await _dbContext.Products.AsNoTracking()
                            .ProjectToType<ProductDto>()
                            .ToListAsync(ct);

        return products;
        
    }
    public async Task<(IEnumerable<ProductDto>, int)> GetPagedAsync(
    int page,
    int pageSize,
    CancellationToken ct)
    {
        var query = _dbContext.Products.AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(p.Id, "SKU-1.0", p.Name, p.Price, true))
            .ToListAsync(ct);

        return (items, totalCount);
    }

}
