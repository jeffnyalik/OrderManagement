using OrderManagement.Models;
using OrderManagement.Services.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.Tests.Services
{
    public class DiscountServiceTests
    {
        [Fact]
        public void NewCustomer_Should_Get_10_Percent_Discount()
        {
            var strategies = new List<IDiscountStrategy> {
                new NewCustomerDiscount(),
                new VipCustomerDiscount()
            };
            var service = new DiscountService(strategies);

            var customer = new Customer { OrderCount = 0, TotalSpent = 0 };
            var order = new Order { Total = 1000 };

            var discount = service.ApplyDiscount(order, customer);

            Assert.Equal(100, discount);

        }

        [Fact]
        public void VipCustomer_Should_Get_15_Percent_Discount()
        {
            // Arrange
            var strategies = new List<IDiscountStrategy> {
                new NewCustomerDiscount(),
                new VipCustomerDiscount()
            };
            var service = new DiscountService(strategies);

            var customer = new Customer { OrderCount = 10, TotalSpent = 7000 };
            var order = new Order { Total = 1000 };

            // Act
            var discount = service.ApplyDiscount(order, customer);

            // Assert
            Assert.Equal(150, discount);
        }

        [Fact]
        public void ReturningCustomer_Should_Get_No_Discount()
        {
            // Arrange
            var strategies = new List<IDiscountStrategy> {
                new NewCustomerDiscount(),
                new VipCustomerDiscount()
            };
            var service = new DiscountService(strategies);

            var customer = new Customer { OrderCount = 3, TotalSpent = 200 };
            var order = new Order { Total = 1000 };

            // Act
            var discount = service.ApplyDiscount(order, customer);

            // Assert
            Assert.Equal(0, discount);
        }


    }
}
