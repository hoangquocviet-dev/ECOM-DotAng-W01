using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return 0;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            int userId = GetUserId();
            var user = await _userService.GetUserProfileAsync(userId);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Name,
                user.PhoneNumber,
                user.Address,
                user.Role
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest request)
        {
            int userId = GetUserId();
            var user = await _userService.UpdateUserProfileAsync(userId, request);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Name,
                user.PhoneNumber,
                user.Address,
                user.Role
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            int userId = GetUserId();
            var success = await _userService.ChangePasswordAsync(userId, request);
            if (!success) return BadRequest("Mật khẩu cũ không chính xác hoặc người dùng không tồn tại.");

            return Ok("Đổi mật khẩu thành công.");
        }
    }
}
