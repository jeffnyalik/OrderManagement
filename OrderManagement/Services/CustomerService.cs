using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Dtos;
using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> CreateCustomerAsync(CustomerCreateDto dto)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                OrderCount = 0,
                TotalSpent = 0
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateCustomerAsync(Guid id, CustomerUpdateDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;
            customer.Name = dto.Name;
            await _context.SaveChangesAsync();
            return customer;
        }
    }
} 