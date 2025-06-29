using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Enums;

namespace OrderManagement.Services
{
    public class OrderAnalyticsService
    {
       private readonly AppDbContext _context;
        public OrderAnalyticsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<object> GetAnalyticsAsync()
        {
            var orders = await _context.Orders.AsNoTracking().ToListAsync();
            if(orders.Count == 0)
            {
                return new
                {
                    AverageOrderValue = 0.0m,
                    AverageFulfillmentTimeHours = 0.0m,
                };
            }

            var averageValue = orders.Average(o => o.Total);
            var fulfilledOrders = orders
           .Where(o => o.Status == OrderStatus.Delivered && o.DeliveredAt != null)
           .ToList();

            var averageFulfillmentTime = fulfilledOrders.Any()
                ? fulfilledOrders.Average(o =>
                      (o.DeliveredAt!.Value - o.CreatedAt).TotalHours)
                : 0;
            return new
            {
                AverageOrderValue = Math.Round(averageValue, 2),
                AverageFulfillmentTimeHours = Math.Round(averageFulfillmentTime, 2)
            };

        }
    }
}
