using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VouchersController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVouchersAsync()
        {
            var vouchers = await _voucherService.GetAllVouchersAsync();
            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherByIdAsync(int id)
        {
            var voucher = await _voucherService.GetVoucherByIdAsync(id);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucherAsync([FromBody] CreateVoucherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingVoucher = await _voucherService.GetVoucherByCodeAsync(request.Code);
            if (existingVoucher != null) return BadRequest("Voucher code already exists.");

            var createdVoucher = await _voucherService.CreateVoucherAsync(request);
            return CreatedAtAction(nameof(GetVoucherByIdAsync), new { id = createdVoucher.Id }, createdVoucher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucherAsync(int id, [FromBody] UpdateVoucherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedVoucher = await _voucherService.UpdateVoucherAsync(id, request);
            if (updatedVoucher == null) return NotFound();

            return Ok(updatedVoucher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucherAsync(int id)
        {
            var success = await _voucherService.DeleteVoucherAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
