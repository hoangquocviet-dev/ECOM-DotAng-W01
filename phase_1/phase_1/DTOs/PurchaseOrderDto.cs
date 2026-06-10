using System;
using System.Collections.Generic;

namespace phase_1.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public IEnumerable<PurchaseOrderDetailDto> Items { get; set; } = new List<PurchaseOrderDetailDto>();
    }

    public class PurchaseOrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        public int SupplierId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public IEnumerable<CreatePurchaseOrderDetailDto> Items { get; set; } = new List<CreatePurchaseOrderDetailDto>();
    }

    public class CreatePurchaseOrderDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
