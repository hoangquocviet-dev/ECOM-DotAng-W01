using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] bool includeInactive = false)
        {
            if (includeInactive)
            {
                var banners = await _bannerService.GetAllAsync();
                return Ok(banners);
            }
            else
            {
                var banners = await _bannerService.GetActiveBannersAsync();
                return Ok(banners);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var banner = await _bannerService.GetByIdAsync(id);
            if (banner == null) return NotFound();
            return Ok(banner);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBannerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var banner = await _bannerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = banner.Id }, banner);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateBannerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var banner = await _bannerService.UpdateAsync(id, request);
            if (banner == null) return NotFound();

            return Ok(banner);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _bannerService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
