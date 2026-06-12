using System.Collections.Generic;

namespace phase_1.DTOs
{
    public class AutoSuggestDto
    {
        public IEnumerable<string> Keywords { get; set; } = new List<string>();
        public IEnumerable<ProductSuggestDto> Products { get; set; } = new List<ProductSuggestDto>();
    }

    public class ProductSuggestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
    }
}
