using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<TopSellingProductDto>> GetTopSellingProductsAsync(int top = 5);
    }
}
