using Catalog.Domain.Shared;
namespace Catalog.Domain.Products;
public sealed class Product
{
    public Guid Id { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { } // EF

    private Product(string sku, string name, decimal price)
    {
        Id = Guid.NewGuid();
        SetSku(sku);
        SetName(name);
        SetPrice(price);
        IsActive = true;
    }

    public static Product Create(string sku, string name, decimal price)
    {
        return new Product(sku, name, price);
    }

    public void SetSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("SKU is required.");

        Sku = sku.Trim();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required.");

        if (name.Length > 200)
            throw new DomainException("Name length invalid.");

        Name = name.Trim();
    }

    public void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Price must be greater than zero.");

        Price = price;
    }

    public void Deactivate() => IsActive = false;
}


