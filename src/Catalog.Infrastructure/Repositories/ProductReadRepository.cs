using Catalog.Application.DTOs;
using Catalog.Application.interfaces;
using Catalog.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace Catalog.Infrastructure.Repositories;

public class ProductReadRepository(
    CatalogDbContext dbContext,
    ILogger<ProductReadRepository> logger
    ) : IProductReadRepository
{
    public async Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();
        var products = await dbContext.Products.AsNoTracking()
                            .ProjectToType<ProductDto>()
                            .ToListAsync(ct);
        stopwatch.Stop();

        logger.LogCritical("----- GetProductsAsync - ElapsedMilliseconds={ElapsedMilliseconds}  --------", stopwatch.ElapsedMilliseconds);
        return products;
    }

    public async Task<(IEnumerable<ProductDto>, int)> GetPagedAsync(int page,  int pageSize, CancellationToken ct)
    {
        var query = dbContext.Products.AsNoTracking();

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
  