﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Dtos;
using OrderManagement.Enums;
using OrderManagement.Models;
using OrderManagement.Services;
using OrderManagement.Services.Discount;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly DiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;

        public OrdersController(
            AppDbContext context, DiscountService discountService,
            IMapper mapper,
            OrderService orderService
            )
        {
            _context = context;
            _discountService = discountService;
            _mapper = mapper;
            _orderService = orderService;
        }

        /// <summary>
        /// Get all orders, optionally filtered by customerId
        /// </summary>
        /// <param name="customerId">Optional filter by customer ID</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        public async Task<IActionResult> GetOrders([FromQuery] Guid? customerId)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .AsNoTracking()
                .AsQueryable();

            if (customerId.HasValue)
                query = query.Where(o => o.CustomerId == customerId);

            var orders = await query.ToListAsync();
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        /// <summary>
        /// Update order status with valid state transitions
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderStatus newStatus)
        {
            var order = await _orderService.UpdateStatusAsync(id, newStatus);
            if (order == null)
                return BadRequest("Invalid status transition or order not found.");
            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        /// <summary>
        /// Get order analytics including average order value and fulfillment time
        /// </summary>
        [HttpGet("analytics")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAnalytics([FromServices] OrderAnalyticsService analytics)
        {
            var data = await analytics.GetAnalyticsAsync();
            return Ok(data);
        }

        /// <summary>
        /// Calculate applicable discount for a customer's order
        /// </summary>
        [HttpPost("{id}/discount")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetDiscount(Guid id)
        {
            var order = await _context.Orders.Include(o => o.Customer)
                                             .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null || order.Customer == null)
                return NotFound();

            var discount = _discountService.ApplyDiscount(order, order.Customer);
            return Ok(discount);
        }

        /// <summary>
        /// Create a new order for a customer
        /// </summary>
        /// <param name="dto">Order creation data</param>
        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var order = await _orderService.CreateOrderAsync(dto);
            if (order == null)
                return BadRequest("Customer not found.");
            var orderDto = _mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, orderDto);
        }
    }
}
