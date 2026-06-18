using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;

namespace phase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashSalesController : ControllerBase
    {
        private readonly IFlashSaleService _flashSaleService;

        public FlashSalesController(IFlashSaleService flashSaleService)
        {
            _flashSaleService = flashSaleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlashSaleDto>>> GetAllFlashSales()
        {
            var result = await _flashSaleService.GetAllFlashSalesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlashSaleDto>> GetFlashSaleById(int id)
        {
            var result = await _flashSaleService.GetFlashSaleByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<ActionResult<FlashSaleDto>> GetActiveFlashSale()
        {
            var result = await _flashSaleService.GetActiveFlashSaleAsync();
            if (result == null) return NotFound("No active flash sale at the moment.");
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FlashSaleDto>> CreateFlashSale([FromBody] CreateFlashSaleRequest request)
        {
            var result = await _flashSaleService.CreateFlashSaleAsync(request);
            return CreatedAtAction(nameof(GetFlashSaleById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateFlashSale(int id, [FromBody] UpdateFlashSaleRequest request)
        {
            var updated = await _flashSaleService.UpdateFlashSaleAsync(id, request);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteFlashSale(int id)
        {
            var deleted = await _flashSaleService.DeleteFlashSaleAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
