namespace Catalog.Api.Contracts;
public class CreateOrderRequest
{
    public string CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

