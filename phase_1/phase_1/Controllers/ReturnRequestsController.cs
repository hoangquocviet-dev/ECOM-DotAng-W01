using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReturnRequestsController : ControllerBase
    {
        private readonly IReturnRequestService _returnService;

        public ReturnRequestsController(IReturnRequestService returnService)
        {
            _returnService = returnService;
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var requests = await _returnService.GetByUserIdAsync(userId);
            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateReturnRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            try
            {
                var result = await _returnService.CreateRequestAsync(userId, request);
                return Ok(result);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _returnService.GetAllAsync();
            return Ok(requests);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var request = await _returnService.GetByIdAsync(id);
            if (request == null) return NotFound("Không tìm thấy yêu cầu hoàn hàng.");
            return Ok(request);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/process")]
        public async Task<IActionResult> ProcessRequest(int id, [FromBody] ProcessReturnRequest request)
        {
            var result = await _returnService.ProcessRequestAsync(id, request);
            if (result == null) return NotFound("Không tìm thấy yêu cầu hoàn hàng.");
            return Ok(result);
        }
    }
}
