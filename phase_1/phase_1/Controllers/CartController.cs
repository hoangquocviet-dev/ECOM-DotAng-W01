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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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
        public async Task<IActionResult> GetCartAsync()
        {
            int userId = GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCartAsync([FromBody] AddToCartRequest request)
        {
            int userId = GetUserId();
            var cart = await _cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);
            return Ok(cart);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItemAsync([FromBody] UpdateCartItemRequest request)
        {
            int userId = GetUserId();
            var cart = await _cartService.UpdateCartItemAsync(userId, request.CartItemId, request.Quantity);
            return Ok(cart);
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] int cartItemId)
        {
            int userId = GetUserId();
            var cart = await _cartService.RemoveFromCartAsync(userId, cartItemId);
            return Ok(cart);
        }
    }
}
