using System;

namespace phase_1.DTOs
{
    public class CreateVoucherRequest
    {
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
    }
}
