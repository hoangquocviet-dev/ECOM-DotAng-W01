using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class PurchaseOrderDetail
    {
        public int Id { get; set; }
        
        public int PurchaseOrderId { get; set; }
        [JsonIgnore]
        public PurchaseOrder? PurchaseOrder { get; set; }

        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
