using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrandsAsync()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandByIdAsync(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBrandAsync([FromBody] CreateBrandRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdBrand = await _brandService.CreateBrandAsync(request);
            return CreatedAtAction(nameof(GetBrandByIdAsync), new { id = createdBrand.Id }, createdBrand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandAsync(int id, [FromBody] UpdateBrandRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedBrand = await _brandService.UpdateBrandAsync(id, request);
            if (updatedBrand == null) return NotFound();

            return Ok(updatedBrand);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrandAsync(int id)
        {
            var success = await _brandService.DeleteBrandAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
