using Catalog.Domain.Products;
namespace Catalog.Application.Products.Commands;
public sealed class CreateProductHandler
{

    public CreateProductHandler()
    {
    }

    public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken ct = default)
    {
        //// Application-level validation (shape, existence)
        //if (string.IsNullOrWhiteSpace(command.Sku))
        //    throw new ApplicationException("SKU is required.");

        //var skuExists = await _db.Products
        //    .AnyAsync(p => p.Sku == command.Sku, ct);

        //if (skuExists)
        //    throw new ApplicationException("SKU must be unique.");

        //// Aggregate creation - domain invariants enforced here
        //var product = Product.Create(command.Sku, command.Name, command.Price);

        //_db.Products.Add(product);
        //await _db.SaveChangesAsync(ct);

        // return product.Id;   
        return new Guid();
    }
}
