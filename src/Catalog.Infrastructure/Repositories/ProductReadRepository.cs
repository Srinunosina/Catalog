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
}
