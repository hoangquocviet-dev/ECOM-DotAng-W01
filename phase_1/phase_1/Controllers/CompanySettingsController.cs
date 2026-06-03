using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.Models;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanySettingsController : ControllerBase
    {
        private readonly ICompanySettingService _service;
        public CompanySettingsController(ICompanySettingService service)
        {
            _service = service;
        }

        // Bất kỳ ai cũng có thể đọc thông tin công ty để hiển thị lên web
        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _service.GetSettingsAsync();
            return Ok(result);
        }

        // Chỉ Admin mới được phép thay đổi thông tin công ty
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] CompanySetting settings)
        {
            var result = await _service.UpdateSettingsAsync(settings);
            return Ok(result);
        }
    }
}
