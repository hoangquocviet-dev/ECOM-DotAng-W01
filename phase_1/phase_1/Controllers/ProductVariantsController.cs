using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantService _variantService;

        public ProductVariantsController(IProductVariantService variantService)
        {
            _variantService = variantService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetVariantsByProductId(int productId)
        {
            var variants = await _variantService.GetVariantsByProductIdAsync(productId);
            return Ok(variants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVariantById(int id)
        {
            var variant = await _variantService.GetVariantByIdAsync(id);
            if (variant == null) return NotFound();
            return Ok(variant);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("product/{productId}")]
        public async Task<IActionResult> CreateVariant(int productId, [FromBody] CreateProductVariantRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var variant = await _variantService.CreateVariantAsync(productId, request);
                return CreatedAtAction(nameof(GetVariantById), new { id = variant.Id }, variant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVariant(int id, [FromBody] UpdateProductVariantRequest request)
        {
            try
            {
                var variant = await _variantService.UpdateVariantAsync(id, request);
                return Ok(variant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariant(int id)
        {
            var result = await _variantService.DeleteVariantAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
