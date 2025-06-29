using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dtos
{
    public class CustomerUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
