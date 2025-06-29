using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dtos
{
    public class OrderCreateDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than zero.")]
        public decimal Total { get; set; }

        // Optional: allows status override (e.g., start as Pending or Shipped)
        public string? Status { get; set; }
    }
}
