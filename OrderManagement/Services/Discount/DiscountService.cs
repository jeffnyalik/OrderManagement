using OrderManagement.Models;

namespace OrderManagement.Services.Discount
{
    public class DiscountService
    {
        private readonly IEnumerable<IDiscountStrategy> _strategies;
        public DiscountService(IEnumerable<IDiscountStrategy> strategies)
        {
            _strategies = strategies;
        }
        public decimal ApplyDiscount(Order order, Customer customer)
        {
            return _strategies.Max(s => s.CalculateDiscount(order, customer));
        }
    }
}
