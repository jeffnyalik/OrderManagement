using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Dtos;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDto>), 200)]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.Customers.AsNoTracking().ToListAsync();
            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDto);
        }

        /// <summary>
        /// Create a new customer.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            customer.Id = Guid.NewGuid();
            customer.OrderCount = 0;
            customer.TotalSpent = 0;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<CustomerDto>(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, resultDto);
        }

        /// <summary>
        /// Update a customer.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            customer.Name = dto.Name;

            await _context.SaveChangesAsync();

            var updatedDto = _mapper.Map<CustomerDto>(customer);
            return Ok(updatedDto);
        }

        /// <summary>
        /// Get a customer by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            var dto = _mapper.Map<CustomerDto>(customer);

            return Ok(dto);
        }
    }
}
