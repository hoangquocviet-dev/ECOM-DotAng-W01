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

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
