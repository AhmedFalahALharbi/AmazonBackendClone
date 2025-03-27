namespace AmazonBackend.DTOs
{
    // DTO for creating a new order
    public class CreateOrderDto
    {
        // List of items in the order
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
