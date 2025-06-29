using OrderManagement.Enums;

namespace OrderManagement.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }

        public bool canTransitionTo(OrderStatus newStatus)
        {
            return (Status == OrderStatus.Pending && newStatus == OrderStatus.Processing) ||
                   (Status == OrderStatus.Processing && newStatus == OrderStatus.Delivered) ||
                   (Status == OrderStatus.Delivered && newStatus == OrderStatus.Completed);
        }
    }
}
