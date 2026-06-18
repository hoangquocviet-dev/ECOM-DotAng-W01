using System;
using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class FlashSaleItem
    {
        public int Id { get; set; }

        public int FlashSaleId { get; set; }
        [JsonIgnore]
        public FlashSale? FlashSale { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public decimal DiscountPrice { get; set; }
        
        public int Quantity { get; set; } 
        public int SoldQuantity { get; set; } 
        
        public int MaxPerCustomer { get; set; } 
    }
}
