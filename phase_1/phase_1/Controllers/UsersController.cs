using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Linq;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = users.Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.Name,
                u.PhoneNumber,
                u.Address,
                u.Role,
                u.IsEmailVerified,
                u.IsLocked
            });
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/lock")]
        public async Task<IActionResult> LockUnlockUserAsync(int id, [FromBody] LockUnlockUserRequest request)
        {
            var success = await _userService.LockUnlockUserAsync(id, request.IsLocked);
            if (!success) return NotFound("Không tìm thấy người dùng.");

            return Ok(new { message = request.IsLocked ? "Tài khoản đã bị khóa." : "Tài khoản đã được mở khóa." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> ChangeUserRoleAsync(int id, [FromBody] ChangeUserRoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Role)) return BadRequest("Role không được để trống.");

            var success = await _userService.ChangeUserRoleAsync(id, request.Role);
            if (!success) return NotFound("Không tìm thấy người dùng.");

            return Ok(new { message = $"Phân quyền thành công. Quyền mới: {request.Role}" });
        }
    }
}
