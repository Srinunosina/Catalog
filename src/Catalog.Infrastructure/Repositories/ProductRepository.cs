using Catalog.Application.interfaces;
using Catalog.Domain.Products;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            //throw new NotImplementedException();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}
