using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<TopSellingProductDto>> GetTopSellingProductsAsync(int top = 5);
    }
}
