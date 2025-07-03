using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Dtos;
using OrderManagement.Enums;
using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> CreateOrderAsync(OrderCreateDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null) return null;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                Total = dto.Total,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateStatusAsync(Guid id, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return null;
            if (!CanTransitionTo(order.Status, newStatus)) return null;
            order.Status = newStatus;
            if (newStatus == OrderStatus.Delivered)
                order.DeliveredAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return order;
        }

        public bool CanTransitionTo(OrderStatus current, OrderStatus next)
        {
            return (current == OrderStatus.Pending && next == OrderStatus.Processing) ||
                   (current == OrderStatus.Processing && next == OrderStatus.Delivered) ||
                   (current == OrderStatus.Delivered && next == OrderStatus.Completed);
        }
    }
} 