using System;
using System.Collections.Generic;

namespace phase_1.DTOs
{
    public class FlashSaleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public List<FlashSaleItemDto> FlashSaleItems { get; set; } = new List<FlashSaleItemDto>();
    }

    public class FlashSaleItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public int SoldQuantity { get; set; }
        public int MaxPerCustomer { get; set; }
    }
}
