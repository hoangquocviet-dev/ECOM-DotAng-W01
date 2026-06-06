using System;

namespace phase_1.DTOs
{
    public class UpdateVoucherRequest
    {
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
    }
}
