using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dtos
{
    public class CustomerCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
