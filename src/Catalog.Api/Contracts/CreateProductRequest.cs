namespace Catalog.Api.Contracts
{
    public sealed class CreateProductRequest
    {
        public string Sku { get; init; } = default!;
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
    }
}
