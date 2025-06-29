namespace OrderManagement.Models
{
    public class Customer : Base
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
