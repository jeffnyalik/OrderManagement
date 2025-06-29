using OrderManagement.Models;

namespace OrderManagement.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Customers.Any() || context.Orders.Any())
            {
                return; // DB has been seeded
            }
            var customer1 = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Alice Smith",
                OrderCount = 2,
                TotalSpent = 150.00m,
                CreatedAt = DateTime.UtcNow
            };
            var customer2 = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Bob Johnson",
                OrderCount = 1,
                TotalSpent = 75.00m,
                CreatedAt = DateTime.UtcNow
            };
            context.Customers.AddRange(customer1, customer2);
            var order1 = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer1.Id,
                Customer = customer1,
                CreatedAt = DateTime.UtcNow,
                Total = 100.00m,
                Status = Enums.OrderStatus.Pending
            };
            var order2 = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer1.Id,
                Customer = customer1,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                DeliveredAt = DateTime.UtcNow.AddDays(-1),
                Total = 50.00m,
                Status = Enums.OrderStatus.Delivered
            };
            var order3 = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer2.Id,
                Customer = customer2,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Total = 75.00m,
                Status = Enums.OrderStatus.Completed
            };
            context.Orders.AddRange(order1, order2, order3);
            context.SaveChanges();
        }
    }
}
