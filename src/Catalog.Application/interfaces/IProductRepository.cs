using Catalog.Domain.Products;

namespace Catalog.Application.interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
    }
}
