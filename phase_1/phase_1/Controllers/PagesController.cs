using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPagesAsync([FromQuery] bool includeInactive = false)
        {
            var result = await _pageService.GetAllPagesAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPageByIdAsync([FromRoute] int id)
        {
            var result = await _pageService.GetPageByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetPageBySlugAsync([FromRoute] string slug)
        {
            var result = await _pageService.GetPageBySlugAsync(slug);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePageAsync([FromBody] CreatePageRequest request)
        {
            var result = await _pageService.CreatePageAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePageAsync([FromRoute] int id, [FromBody] UpdatePageRequest request)
        {
            var result = await _pageService.UpdatePageAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePageAsync([FromRoute] int id)
        {
            var result = await _pageService.DeletePageAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
