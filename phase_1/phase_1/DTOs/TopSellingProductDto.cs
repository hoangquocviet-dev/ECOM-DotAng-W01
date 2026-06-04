namespace phase_1.DTOs
{
    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalSoldQuantity { get; set; }
        public decimal TotalRevenueGenerated { get; set; }
    }
}
