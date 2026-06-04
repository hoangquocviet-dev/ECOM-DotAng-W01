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
    }
}
