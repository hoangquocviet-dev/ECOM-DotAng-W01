using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public Users? User { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "COD";
        public string PaymentStatus { get; set; } = "Pending";

        public decimal DiscountAmount { get; set; } = 0;
        public int? VoucherId { get; set; }
        [JsonIgnore]
        public Voucher? Voucher { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
