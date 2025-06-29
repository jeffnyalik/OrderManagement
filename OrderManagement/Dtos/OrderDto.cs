using OrderManagement.Enums;

namespace OrderManagement.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public OrderStatus Status { get; set; }

        public CustomerDto? Customer { get; set; }
    }
}
