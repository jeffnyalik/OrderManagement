using OrderManagement.Models;

namespace OrderManagement.Services.Discount
{
    public class VipCustomerDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order, Customer customer)
        {
            return customer.TotalSpent >= 5000.00m ? order.Total * 0.15m : 0.0m;
        }
    }
}
