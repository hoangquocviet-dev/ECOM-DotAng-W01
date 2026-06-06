using System;

namespace phase_1.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
    }
}
