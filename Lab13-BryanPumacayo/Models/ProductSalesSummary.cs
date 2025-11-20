namespace Lab13_BryanPumacayo.Models
{
    public class ProductSalesSummary
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}