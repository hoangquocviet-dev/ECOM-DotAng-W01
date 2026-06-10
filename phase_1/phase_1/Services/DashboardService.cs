using phase_1.DTOs;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            return await _dashboardRepository.GetSummaryAsync();
        }

        public async Task<IEnumerable<TopSellingProductDto>> GetTopSellingProductsAsync(int top = 5)
        {
            return await _dashboardRepository.GetTopSellingProductsAsync(top);
        }

        public async Task<IEnumerable<RevenueDataPointDto>> GetRevenueChartAsync(string period)
        {
            return await _dashboardRepository.GetRevenueChartAsync(period);
        }
    }
}
