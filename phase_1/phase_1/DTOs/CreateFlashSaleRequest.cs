using System;
using System.Collections.Generic;

namespace phase_1.DTOs
{
    public class CreateFlashSaleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; } = true;
        public List<AddFlashSaleItemRequest> Items { get; set; } = new List<AddFlashSaleItemRequest>();
    }

    public class AddFlashSaleItemRequest
    {
        public int ProductId { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public int MaxPerCustomer { get; set; }
    }
}
