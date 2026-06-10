using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var summary = new DashboardSummaryDto
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalRevenue = await _context.Orders
                                    .Where(o => o.Status != "Cancelled")
                                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m
            };

            return summary;
        }

        public async Task<IEnumerable<TopSellingProductDto>> GetTopSellingProductsAsync(int top = 5)
        {
            var query = await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .Where(od => od.Order.Status != "Cancelled")
                .GroupBy(od => new { od.ProductId, od.Product.Name })
                .Select(g => new TopSellingProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    TotalSoldQuantity = g.Sum(od => od.Quantity),
                    TotalRevenueGenerated = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .OrderByDescending(dto => dto.TotalSoldQuantity)
                .Take(top)
                .ToListAsync();

            return query;
        }

        public async Task<IEnumerable<RevenueDataPointDto>> GetRevenueChartAsync(string period)
        {
            var orders = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .ToListAsync();

            if (string.Equals(period, "month", System.StringComparison.OrdinalIgnoreCase))
            {
                return orders
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Select(g => new RevenueDataPointDto
                    {
                        Label = $"{g.Key.Month:D2}/{g.Key.Year}",
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(x => x.Label)
                    .ToList();
            }
            else if (string.Equals(period, "week", System.StringComparison.OrdinalIgnoreCase))
            {
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                return orders
                    .GroupBy(o => new {
                        o.OrderDate.Year, 
                        Week = cal.GetWeekOfYear(o.OrderDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday)
                    })
                    .Select(g => new RevenueDataPointDto
                    {
                        Label = $"W{g.Key.Week:D2}/{g.Key.Year}",
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(x => x.Label)
                    .ToList();
            }
            else // day
            {
                return orders
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new RevenueDataPointDto
                    {
                        Label = g.Key.ToString("yyyy-MM-dd"),
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(x => x.Label)
                    .ToList();
            }
        }

        public async Task<CustomerBehaviorMetricsDto> GetCustomerBehaviorMetricsAsync()
        {
            var customerStats = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    OrderCount = g.Count(),
                    TotalSpent = g.Sum(o => o.TotalAmount)
                })
                .ToListAsync();

            if (!customerStats.Any())
            {
                return new CustomerBehaviorMetricsDto();
            }

            int totalCustomers = customerStats.Count;
            int returningCustomers = customerStats.Count(c => c.OrderCount > 1);
            decimal totalRevenue = customerStats.Sum(c => c.TotalSpent);

            double retentionRate = totalCustomers > 0 ? System.Math.Round((double)returningCustomers / totalCustomers * 100, 2) : 0;
            decimal averageLifetimeValue = totalCustomers > 0 ? System.Math.Round(totalRevenue / totalCustomers, 2) : 0;

            return new CustomerBehaviorMetricsDto
            {
                TotalCustomers = totalCustomers,
                ReturningCustomers = returningCustomers,
                RetentionRate = retentionRate,
                AverageLifetimeValue = averageLifetimeValue
            };
        }
    }
}
