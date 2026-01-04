namespace Catalog.Application.DTOs;
public record ProductDto(
    Guid Id,
    string Sku,
    string Name,
   // decimal Price,
    bool IsActive
);
