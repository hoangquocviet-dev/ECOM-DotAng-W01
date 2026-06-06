using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetImagesByProductIdAsync(int productId)
        {
            var images = await _productImageService.GetImagesByProductIdAsync(productId);
            return Ok(images);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddImageAsync([FromBody] CreateProductImageRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdImage = await _productImageService.AddImageAsync(request);
            return Ok(createdImage);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageAsync(int id)
        {
            var success = await _productImageService.DeleteImageAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
