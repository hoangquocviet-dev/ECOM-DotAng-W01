using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    public class UpdateProductVariantRequest
    {
        public string? SKU { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }
        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }
    }
}
