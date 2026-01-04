namespace Catalog.Application.Products.Commands;
public sealed class CreateProductCommand
{
    public string Sku { get; init; } = default!;
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
}
