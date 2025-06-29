using OrderManagement.Models;

namespace OrderManagement.Services.Discount
{
    public class NewCustomerDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order, Customer customer)
        {
            return customer.OrderCount == 0 ? order.Total * 0.10m : 0.0m;
        }
    }
}
