using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
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

        [HttpGet]
        public async Task<IActionResult> GetUserWishlistAsync()
        {
            int userId = GetUserId();
            var wishlist = await _wishlistService.GetUserWishlistAsync(userId);
            return Ok(wishlist);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToWishlistAsync(int productId)
        {
            int userId = GetUserId();
            var success = await _wishlistService.AddToWishlistAsync(userId, productId);
            if (!success)
            {
                return BadRequest("Product not found or could not be added.");
            }
            return Ok(new { Message = "Product added to wishlist successfully." });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishlistAsync(int productId)
        {
            int userId = GetUserId();
            var success = await _wishlistService.RemoveFromWishlistAsync(userId, productId);
            if (!success)
            {
                return NotFound("Product not found in wishlist.");
            }
            return Ok(new { Message = "Product removed from wishlist successfully." });
        }
    }
}
