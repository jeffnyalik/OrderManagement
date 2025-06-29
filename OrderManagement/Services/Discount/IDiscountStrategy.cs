using OrderManagement.Models;

namespace OrderManagement.Services.Discount
{
    public interface IDiscountStrategy
    {
       decimal CalculateDiscount(Order order, Customer customer);
    }
}
