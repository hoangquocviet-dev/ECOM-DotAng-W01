using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummaryAsync()
        {
            var summary = await _dashboardService.GetSummaryAsync();
            return Ok(summary);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopSellingProductsAsync([FromQuery] int top = 5)
        {
            var topProducts = await _dashboardService.GetTopSellingProductsAsync(top);
            return Ok(topProducts);
        }

        [HttpGet("revenue-chart")]
        public async Task<IActionResult> GetRevenueChartAsync([FromQuery] string period = "day")
        {
            var data = await _dashboardService.GetRevenueChartAsync(period);
            return Ok(data);
        }
    }
}
