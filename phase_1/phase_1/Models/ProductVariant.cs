using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public string SKU { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Size { get; set; }
        
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
