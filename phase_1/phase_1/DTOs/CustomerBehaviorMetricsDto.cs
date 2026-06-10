using System;

namespace phase_1.DTOs
{
    public class CustomerBehaviorMetricsDto
    {
        public double RetentionRate { get; set; }
        public decimal AverageLifetimeValue { get; set; }
        public int TotalCustomers { get; set; }
        public int ReturningCustomers { get; set; }
    }
}
