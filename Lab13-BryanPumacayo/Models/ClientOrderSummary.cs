namespace Lab13_BryanPumacayo.Models
{
    public class ClientOrderSummary
    {
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OrdersCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}