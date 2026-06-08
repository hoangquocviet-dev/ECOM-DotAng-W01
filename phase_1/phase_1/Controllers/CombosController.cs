using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase
    {
        private readonly IComboService _comboService;

        public CombosController(IComboService comboService)
        {
            _comboService = comboService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCombos([FromQuery] bool includeInactive = false)
        {
            var combos = await _comboService.GetAllCombosAsync(includeInactive);
            return Ok(combos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComboById(int id)
        {
            var combo = await _comboService.GetComboByIdAsync(id);
            if (combo == null) return NotFound("Không tìm thấy Combo.");
            return Ok(combo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCombo([FromBody] CreateComboRequest request)
        {
            var combo = await _comboService.CreateComboAsync(request);
            return CreatedAtAction(nameof(GetComboById), new { id = combo.Id }, combo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCombo(int id, [FromBody] UpdateComboRequest request)
        {
            var combo = await _comboService.UpdateComboAsync(id, request);
            if (combo == null) return NotFound("Không tìm thấy Combo.");
            return Ok(combo);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            var success = await _comboService.DeleteComboAsync(id);
            if (!success) return NotFound("Không tìm thấy Combo.");
            return NoContent();
        }
    }
}
